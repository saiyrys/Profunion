﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using profunion.Applications.Interface.IEvents.IService;
using profunion.Domain.Models.EventModels;
using profunion.Domain.Models.NewsModels;
using profunion.Domain.Persistance;
using profunion.Infrastructure.Data;
using profunion.Infrastructure.Persistance.Repository;
using profunion.Shared.Common.Interfaces;
using profunion.Shared.Dto.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Applications.Services.Events
{
    public class EventWriterService : IEventWriterService
    {
        private readonly IEventRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUpdateMethods _update;
        private readonly IFileRepository _fileRepository;
        private readonly ApplicationDbContext _context;

        public EventWriterService(IEventRepository repository, IMapper mapper, IUpdateMethods update, IFileRepository fileRepository, ApplicationDbContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _update = update;
            _fileRepository = fileRepository;
            _context = context;
        }
        
        public async Task<bool> CreateEvents(CreateEventDto eventsCreate, CancellationToken cancellation)
        {
            var eventsGet = await _repository.GetAllAsync();

            if (eventsCreate == null)
                throw new ArgumentNullException();

            if (eventsCreate.date < DateTime.Now)
                throw new ArgumentNullException();

            var eventsMap = _mapper.Map<Event>(eventsCreate);
            
            if (eventsCreate.categoriesId?.Any() == true)
            {
                eventsMap.EventCategories = eventsCreate.categoriesId
                    .Select(c => new EventCategories { CategoriesId = c, eventId = eventsMap.eventId })
                    .ToList();
            } 
            
            if (eventsCreate.imagesId?.Any() == true)
            {
                eventsMap.EventUploads = eventsCreate.imagesId
                    .Select(c => new EventUploads { fileId = c, eventId = eventsMap.eventId })
                    .ToList();
            }
            
            if (!await _repository.CreateEntityAsync(eventsMap))
            {
                throw new InvalidOperationException("Что то пошло не так при создании");
            } 
            
            return true;
        }

        public async Task<bool> UpdateEvents(string eventId, UpdateEventDto? updateEvent, CancellationToken cancellation)
        {
            var eventDate = updateEvent.date.ToString();

            if (string.IsNullOrWhiteSpace(eventDate))
            {
                eventDate = null; 
            }

            await _update.UpdateEntity<Event, UpdateEventDto, string>(eventId, updateEvent);

            if (updateEvent.categoriesId != null)
            {
                var eventCategories = _context.EventCategories.Where(ec => ec.eventId == eventId).ToList();
                _context.EventCategories.RemoveRange(eventCategories);

                var newCategories = updateEvent.categoriesId.Select(categoryId => new EventCategories
                {
                    eventId = eventId,
                    CategoriesId = categoryId
                });

                _context.EventCategories.AddRange(newCategories);
            }

            // Удаляем старые фото перед добавлением новых
            if (updateEvent.imagesId != null && updateEvent.imagesId.Any())
            {
                var existingUploads = _context.EventUploads.Where(eu => eu.eventId == eventId).ToList();
                _context.EventUploads.RemoveRange(existingUploads);

                var newUploads = updateEvent.imagesId.Select(uploadId => new EventUploads
                {
                    eventId = eventId,
                    fileId = uploadId
                });

                _context.EventUploads.AddRange(newUploads);
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteEvents(string eventId, CancellationToken cancellation)
        {
            var eventToDelete = await _repository.GetByIdAsync(eventId);

            if (eventToDelete == null)
                throw new ArgumentException();

            var eventCategory = await _context.EventCategories
            .AsNoTracking()
            .ToListAsync(cancellation);

            _context.EventCategories.RemoveRange(eventCategory);

            await _fileRepository.DeleteEventFile(eventId);

            await _context.SaveChangesAsync(cancellation);

            if (!await _repository.DeleteEntity(eventToDelete))
            {
                throw new ArgumentException("Ошибка удаления ивента");
            }

            return true;
        }
    }
}
