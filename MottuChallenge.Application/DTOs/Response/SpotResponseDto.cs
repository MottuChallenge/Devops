using MottuChallenge.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
namespace MottuChallenge.Application.DTOs.Response
{
    public class SpotResponseDto
    {
        public Guid SpotId { get; set; }
        public Guid SectorId { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public SpotStatus Status { get; set; }
        public Guid? MotorcycleId { get; set; }
    }
}
