﻿namespace ItemManagementService.Domain.Entities;

public interface ITradableItem
{
    public Guid Id { get; set; }
    public Guid IconId { get; set; }
}