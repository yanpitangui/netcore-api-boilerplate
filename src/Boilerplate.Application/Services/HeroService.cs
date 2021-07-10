using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Boilerplate.Application.DTOs;
using Boilerplate.Application.DTOs.Hero;
using Boilerplate.Application.Extensions;
using Boilerplate.Application.Filters;
using Boilerplate.Application.Interfaces;
using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Boilerplate.Application.Services
{
    public class HeroService : IHeroService
    {
        private readonly IHeroRepository _heroRepository;

        private readonly IMapper _mapper;

        public HeroService(IMapper mapper, IHeroRepository heroRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _heroRepository = heroRepository ?? throw new ArgumentNullException(nameof(heroRepository));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing) _heroRepository.Dispose();
        }

        #region Hero Methods

        public async Task<PaginatedList<GetHeroDto>> GetAllHeroes(GetHeroesFilter filter)
        {
            filter ??= new GetHeroesFilter();
            var heroes = _heroRepository
                .GetAll()
                .WhereIf(!string.IsNullOrEmpty(filter.Name), x => EF.Functions.Like(x.Name, $"%{filter.Name}%"))
                .WhereIf(!string.IsNullOrEmpty(filter.Nickname), x => EF.Functions.Like(x.Nickname, $"%{filter.Nickname}%"))
                .WhereIf(filter.Age != null, x => x.Age == filter.Age)
                .WhereIf(filter.HeroType != null, x => x.HeroType == filter.HeroType)
                .WhereIf(!string.IsNullOrEmpty(filter.Team), x => x.Team == filter.Team)
                .WhereIf(!string.IsNullOrEmpty(filter.Individuality), x => EF.Functions.Like(x.Individuality, $"%{filter.Individuality}%"));
            return await _mapper.ProjectTo<GetHeroDto>(heroes).ToPaginatedListAsync(filter.CurrentPage, filter.PageSize);
        }

        public async Task<GetHeroDto> GetHeroById(Guid id)
        {
            return _mapper.Map<GetHeroDto>(await _heroRepository.GetById(id));
        }

        public async Task<GetHeroDto> CreateHero(CreateHeroDto hero)
        {
            var created = _heroRepository.Create(_mapper.Map<Hero>(hero));
            await _heroRepository.SaveChangesAsync();
            return _mapper.Map<GetHeroDto>(created);
        }

        public async Task<GetHeroDto> UpdateHero(Guid id, UpdateHeroDto updatedHero)
        {
            var originalHero = await _heroRepository.GetById(id);
            if (originalHero == null) return null;

            originalHero.Name = updatedHero.Name;
            originalHero.Nickname = updatedHero.Nickname;
            originalHero.Team = updatedHero.Team;
            originalHero.Individuality = updatedHero.Individuality;
            originalHero.Age = updatedHero.Age;
            originalHero.HeroType = updatedHero.HeroType;
            _heroRepository.Update(originalHero);
            await _heroRepository.SaveChangesAsync();
            return _mapper.Map<GetHeroDto>(originalHero);
        }

        public async Task<bool> DeleteHero(Guid id)
        {
           await _heroRepository.Delete(id);
           return await _heroRepository.SaveChangesAsync() > 0;
        }

        #endregion
    }
}
