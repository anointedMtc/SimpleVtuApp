﻿namespace SharedKernel.Domain;

public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTimeOffset Created { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTimeOffset? Deleted { get; set; }
    public string? DeletedBy { get; set; }
}
