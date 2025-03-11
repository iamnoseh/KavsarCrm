using Domain.Dtos.Colleague;
using Infrastructure.Interfaces;
using Infrastructure.Responses;
using Infrastructure.Seed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ColleagueController (IColleagueService service) : ControllerBase
{
    [HttpGet("colleagueWithIcons")]
    public async Task<Response<List<GetColleagueWhitKnowingIcons>>> GetColleagueWithIcons(string language = "En") => 
        await service.GetColleaguesWithKnowingIcons(language);
    
    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> CreateColleague(CreateColleague request) => await service.CreateColleague(request);
   
    [HttpGet("colleagueWithIcon/{id}")]
    public async Task<Response<GetColleague>> GetColleagueId(int id,string language = "En") => await service.GetColleagueById(id,language);
    
    [HttpPut]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> UpdateColleague(EditColleague request) => await service.EditColleague(request);
    
    [HttpDelete]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> DeleteColleague(int id) => await service.DeleteColleague(id);
    
    [HttpGet] 
    public async Task<Response<GetColleagueWhitKnowingIcons>> GetColleagueBy(int id , string language = "En") => await service.GetColleagueWithKnowingIcon(id,language);
    
}