﻿using Api.Interfaces;
using Api.Models.DTOs;
using ApplicationLayer;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJwtHandler _jwtHandler;
        public StudentController(IUnitOfWork unitOfWork, IMapper mapper, IJwtHandler jwtHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtHandler = jwtHandler;
        }

        [HttpGet("{id}")]
        //[Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStudentsByCourseId(Guid id)
        {
            try
            {
                Request.Headers.TryGetValue("Authorization", out var values);
                var accountId = _jwtHandler.GetAccountIdFromJwt(values);
                var account = await _unitOfWork._accountRepository.GetByIdAsync(new Guid(accountId));

                if (account == null)
                {
                    return NotFound("User associated with this account is not found");
                }
                var students = await _unitOfWork._studentRepository.GetAllAsync();
                var studentCourse = await _unitOfWork._studentCourseRepository.GetByCourseIdAsync(id);
                var user = await _unitOfWork._userRepository.GetByAccountIdAsync(account.AccountId);
                if (!studentCourse.Any())
                {
                    return NoContent();
                }
                var result = studentCourse.Join(students, sc => sc.StudentId, s => s.StudentId, (sc, c) => _mapper.Map<StudentDTO>((sc, c, user)));
                return Ok(JsonConvert.SerializeObject(result, Formatting.Indented));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }

    }
}
