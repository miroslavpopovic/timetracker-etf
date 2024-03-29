﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TimeTrackerEtf.Data;
using TimeTrackerEtf.Domain;
using TimeTrackerEtf.Models;

namespace TimeTrackerEtf.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/time-entries/")]
    public class TimeEntriesController : Controller
    {
        private readonly TimeTrackerDbContext _dbContext;
        private readonly ILogger<TimeEntriesController> _logger;

        public TimeEntriesController(
            TimeTrackerDbContext dbContext,
            ILogger<TimeEntriesController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TimeEntryModel>> GetById(long id)
        {
            _logger.LogInformation(
                $"Getting a time entry with id: {id}");

            var timeEntry = await _dbContext.TimeEntries
                .Include(x => x.Project)
                .Include(x => x.Project.Client)
                .Include(x => x.User)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (timeEntry == null)
            {
                return NotFound();
            }

            return TimeEntryModel.FromTimeEntry(timeEntry);
        }

        // /time-entries/user/2/2019/7
        // instead of /time-entries?user-id=w&year=2019&month=7
        [HttpGet("user/{userId}/{year}/{month}")]
        public async Task<ActionResult<TimeEntryModel[]>> GetByUserAndMonth(
            long userId, int year, int month)
        {
            _logger.LogInformation(
                $"Getting all time entries for month {year}-{month} for user with id {userId}");

            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);

            var timeEntries = await _dbContext.TimeEntries
                .Include(x => x.User)
                .Include(x => x.Project)
                .Include(x => x.Project.Client)
                .Where(x => x.User.Id == userId &&
                    x.EntryDate >= startDate &&
                    x.EntryDate < endDate)
                .OrderBy(x => x.EntryDate)
                .ToListAsync();

            return timeEntries
                .Select(TimeEntryModel.FromTimeEntry)
                .ToArray();
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<TimeEntryModel>>> GetPage(
            int page = 1, int size = 5)
        {
            _logger.LogInformation(
                $"Getting a page {page} of time entries with page size {size}");

            var timeEntries = await _dbContext.TimeEntries
                .Include(x => x.Project)
                .Include(x => x.Project.Client)
                .Include(x => x.User)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            var totalCount = await _dbContext.TimeEntries.CountAsync();
            return new PagedList<TimeEntryModel>
            {
                Items = timeEntries.Select(TimeEntryModel.FromTimeEntry),
                Page = page,
                PageSize = size,
                TotalCount = totalCount
            };
        }


        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            _logger.LogDebug(
                $"Deleting time entries with id {id}");

            var timeEntry = await _dbContext.TimeEntries.FindAsync(id);

            if (timeEntry == null)
            {
                return NotFound();
            }

            _dbContext.TimeEntries.Remove(timeEntry);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<TimeEntryModel>> Create(
            TimeEntryInputModel model)
        {
            _logger.LogDebug(
                $"Creating a new time entry for user {model.UserId}, project {model.ProjectId} and date {model.EntryDate}");

            var user = await _dbContext.Users.FindAsync(model.UserId);
            var project = await _dbContext.Projects
                .Include(x => x.Client) // Necessary for mapping to TimeEntryModel later
                .SingleOrDefaultAsync(x => x.Id == model.ProjectId);

            if (user == null || project == null)
            {
                return NotFound();
            }

            var timeEntry = new TimeEntry
            {
                User = user,
                Project = project,
                HourRate = user.HourRate
            };
            model.MapTo(timeEntry);

            await _dbContext.TimeEntries.AddAsync(timeEntry);
            await _dbContext.SaveChangesAsync();

            var resultModel = TimeEntryModel.FromTimeEntry(timeEntry);

            return CreatedAtAction(
                nameof(GetById), "TimeEntries",
                new { id = timeEntry.Id }, resultModel);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<TimeEntryModel>> Update(
            long id, TimeEntryInputModel model)
        {
            _logger.LogDebug($"Updating time entry with id {id}");

            var timeEntry = await _dbContext.TimeEntries
                .Include(x => x.User)
                .Include(x => x.Project)
                .Include(x => x.Project.Client)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (timeEntry == null)
            {
                return NotFound();
            }

            model.MapTo(timeEntry);

            _dbContext.TimeEntries.Update(timeEntry);
            await _dbContext.SaveChangesAsync();

            return TimeEntryModel.FromTimeEntry(timeEntry);
        }
    }
}
