using AutoMapper;
using Backend_Service.Application.Interfaces;
using Backend_Service.Domain.DTOs;
using Backend_Service.Domain.Entities;
using Backend_Service.Infrastructure.Interfaces;

namespace Backend_Service.Application.Services;

public class LabelService : ILabelService
{
    private readonly ILabelRepository _labelRepository;
    private readonly IMapper _mapper;
    
    public LabelService(ILabelRepository labelRepository, IMapper mapper)
    {
        _labelRepository = labelRepository;
        _mapper = mapper;
    }
    
    public async Task<LabelDto[]> GetLabels()
    {
        var labels = await _labelRepository.GetLabels();
        
        var labelDtos = _mapper.Map<LabelDto[]>(labels);

        return labelDtos;
    }

    public async Task<LabelDto> GetLabel(Guid labelId)
    {
        var label = await _labelRepository.GetLabel(labelId);
        var labelDto = _mapper.Map<LabelDto>(label);
        return labelDto;
    }

    public async Task<LabelDto> GetLabelWithImages(Guid labelId)
    {
        var label = await _labelRepository.GetLabelWithImages(labelId);
        var labelDto = _mapper.Map<LabelDto>(label);
        return labelDto;
    }

    public async Task<LabelDto> CreateLabel(LabelDto label)
    {
        var labelToCreate = _mapper.Map<Label>(label);
        var createdLabel = await _labelRepository.CreateLabel(labelToCreate);
        var createdLabelDto = _mapper.Map<LabelDto>(createdLabel);
        return createdLabelDto;
    }

    public async Task<LabelDto> UpdateLabel(LabelDto label)
    {
        var labelToUpdate = _mapper.Map<Label>(label);
        var updatedLabel = await _labelRepository.UpdateLabel(labelToUpdate);
        var updatedLabelDto = _mapper.Map<LabelDto>(updatedLabel);
        return updatedLabelDto;
    }

    public Task DeleteLabel(Guid labelId)
    {
        _labelRepository.DeleteLabel(labelId);
        return Task.CompletedTask;
    }
}