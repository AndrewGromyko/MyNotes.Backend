﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using MyNotes.Application.Common.Exceptions;
using MyNotes.Domain.Interfaces;
using MyNotes.Domain.Models;
using MyNotes.Domain.Models.Commands;

namespace MyNotes.Application.Notes.Commands.UpdateNote
{
    public class UpdateNoteCommandHandler
        :IRequestHandler<UpdateNoteCommand>
    {
        private readonly INotesDbContext _dbContext;

        public UpdateNoteCommandHandler(INotesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdateNoteCommand request,
            CancellationToken cancellationToken)
        {
            var entity =
                await _dbContext.Notes.FirstOrDefaultAsync(note =>
                    note.Id == request.Id, cancellationToken);

            if (entity == null || entity.UserId != request.UserId)
            {
                throw new NotFoundException(nameof(Note), request.Id);
            }

            entity.Details = request.Details;
            entity.Title = request.Title;
            entity.EditDate = DateTime.Now;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
