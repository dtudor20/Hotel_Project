using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.Domain.Entities;

public class Room
{
    public int Id { get; set; }

    [MaxLength(10)]
    public string RoomNumber { get; set; } = string.Empty;

    [MaxLength(50)]
    public string RoomType { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal PricePerNight { get; set; }

    public int Capacity { get; set; }
    public bool IsAvailable { get; set; } = true;

    [MaxLength(255)]
    public string? PhotoPath { get; set; }
}