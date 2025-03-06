using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Branch : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string BranchNameTj { get; set; }
    [Required]
    [MaxLength(100)]
    public string AddressTj { get; set; }
    [Required]
    [MaxLength(100)]
    public string BranchNameRu { get; set; }
    [Required]
    [MaxLength(100)]
    public string AddressRu { get; set; }
    [Required]
    [MaxLength(100)]
    public string BranchNameEn { get; set; }
    [Required]
    [MaxLength(100)]
    public string AddressEn { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    [Required]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }
}