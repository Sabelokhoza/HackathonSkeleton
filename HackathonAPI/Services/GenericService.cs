using AutoMapper;
using AutoMapper.QueryableExtensions;
using HackathonAPI.Interfaces;
using HackathonAPI.Data;
using HackathonAPI.Models;
using HackathonAPI.Parameters;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace HackathonAPI.Services
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GenericService(AppDbContext appDbContext, IMapper mapper)
        {
            _context = appDbContext;
            _mapper = mapper;
        }


        public async Task<TResult> AddAsync<TSource, TResult>(TSource source)
        {
            var entity = _mapper.Map<T>(source);

            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<TResult>(entity);
        }


        public async Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters)
        {
            int totalSize = await _context.Set<T>().CountAsync();
            var result = await _context.Set<T>()
                .Skip(queryParameters.StartIndex)
                .Take(queryParameters.PageSize)
                .ProjectTo<TResult>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PagedResult<TResult>()
            {
                TotalCount = totalSize,
                PageIndex = queryParameters.StartIndex,
                PageSize = queryParameters.PageSize,
                Result = result
            };
        }

        public async Task<List<TResult>> GetAllAsync<TResult>()
        {
            return await _context.Set<T>()
                .ProjectTo<TResult>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

       

        public async Task<TResult> GetByIdAsync<TResult>(int? id)
        {
            var result = await _context.Set<T>().FindAsync(id);

            if (result is null)
            {
               throw new Exception("Not found ");
            }

            return _mapper.Map<TResult>(result);
        }

        public async Task RemoveAsync(int? id)
        {
            var entity = await GetByIdAsync<T>(id);

            if (entity is null)
            {
                throw new Exception("Not found ");
            }

                _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();

        }

     

        public async Task UpdateAsync<TSource>(int id, TSource source)
        {
            var entity = await GetByIdAsync<T>(id);
           
            _mapper.Map(source, entity);

            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
