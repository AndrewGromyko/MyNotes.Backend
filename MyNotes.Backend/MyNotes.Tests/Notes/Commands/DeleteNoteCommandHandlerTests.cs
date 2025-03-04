﻿using MyNotes.Application.Common.Exceptions;
using MyNotes.Application.Notes.Commands.CreateNote;
using MyNotes.Application.Notes.Commands.DeleteNote;
using MyNotes.Domain.Models.Commands;
using MyNotes.Tests.Common;

namespace MyNotes.Tests.Notes.Commands
{
    public class DeleteNoteCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task DeleteNoteCommandHandler_Success()
        {
            //Arrange
            var handler = new DeleteNoteCommandHandler(Context);

            //Act
            await handler.Handle(new DeleteNoteCommand
            {
                Id = MyNotesContextFactory.NoteIdForDelete,
                UserId = MyNotesContextFactory.UserAId,
            }, CancellationToken.None);

            //Assert
            Assert.Null(Context.Notes.SingleOrDefault(note =>
                note.Id == MyNotesContextFactory.NoteIdForDelete));
        }

        [Fact]
        public async Task DeleteNoteCommandHandler_FailOnWrongId()
        {
            //Arrange
            var handler = new DeleteNoteCommandHandler(Context);

            //Act

            //Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new DeleteNoteCommand
                    {
                        Id = Guid.NewGuid(),
                        UserId = MyNotesContextFactory.UserAId
                    }, CancellationToken.None));
        }

        [Fact]
        public async Task DeleteNoteCommandHandler_FailOnWrongUserId()
        {
            //Arrange
            var deleteHandler = new DeleteNoteCommandHandler(Context);
            var createHandler = new CreateNoteCommandHandler(Context);
            var noteId = await createHandler.Handle(
                new CreateNoteCommand
                {
                    Title = "NoteTitle",
                    Details = "NoteDetails",
                    UserId = MyNotesContextFactory.UserAId
                }, CancellationToken.None);

            //Act

            //Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await deleteHandler.Handle(
                    new DeleteNoteCommand
                    {
                        Id = noteId,
                        UserId = MyNotesContextFactory.UserBId
                    }, CancellationToken.None));
        }
    }
}
