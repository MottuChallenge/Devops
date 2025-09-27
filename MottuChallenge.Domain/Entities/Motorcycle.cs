using MottuChallenge.Domain.Enums;
using MottuChallenge.Domain.Exceptions;
using MottuChallenge.Domain.Validations;
using System.Text.RegularExpressions;
using static System.Collections.Specialized.BitVector32;

namespace MottuChallenge.Domain.Entities
{
    public class Motorcycle
    {
        public Guid Id { get; private set; }
        public string Model { get; private set; }
        public EngineType EngineType { get; private set; }
        public string Plate { get; private set; }
        public DateTime LastRevisionDate { get; private set; }
        public Guid? SpotId { get; private set; }
        public Spot Spot { get; private set; }

        public Motorcycle(string model, EngineType engineType, string plate, DateTime lastRevisionDate)
        {
            ValidatePlate(plate);
            ValidateEngineType(engineType);
            ValidateLastRevisionDate(lastRevisionDate);
            this.Id = Guid.NewGuid();
            this.Model = model;
            this.EngineType = engineType;
            this.Plate = plate;
            this.LastRevisionDate = lastRevisionDate;
        }

        private Motorcycle() { }

        public void SetSpot(Spot spot)
        {
            if (spot == null)
                throw new DomainValidationException("Spot cannot be null", nameof(spot), nameof(Motorcycle));
            Spot = spot;
            SpotId = spot.SpotId;
        }

        public void UpdateModel(string model)
        {
            Guard.AgainstNullOrWhitespace(model, nameof(model), nameof(Motorcycle));
            Model = model;
        }

        public void UpdateEngineType(EngineType engineType)
        {
            ValidateEngineType(engineType);
            EngineType = engineType;
        }

        public void UpdatePlate(string plate)
        {
            ValidatePlate(plate);
            Plate = plate;
        }

        public void UpdateLastRevisionDate(DateTime lastRevisionDate)
        {
            ValidateLastRevisionDate(lastRevisionDate);
            LastRevisionDate = lastRevisionDate;
        }

        public void RemoveSpot()
        {
            if (Spot == null) return;
            Spot.RemoveMotorcycle();
            Spot = null;
            SpotId = null;
            
        }

        private void ValidatePlate(string plate)
        {
            Guard.AgainstNullOrWhitespace(plate, nameof(plate), nameof(Motorcycle));

            if (plate.Length != 8)
            {
                throw new PlateException("License plate must have 8 characters");
            }

            if (char.IsLetter(plate, 4))
            {
                var padraoMercosul = new Regex("[a-zA-Z]{3}[0-9]{1} [a-zA-Z]{1}[0-9]{2}");
                if (!padraoMercosul.IsMatch(plate)) throw new PlateException("Placa inválida");
            }
            else
            {
                var padraoNormal = new Regex("[a-zA-Z]{3}-[0-9]{4}");
                if (!padraoNormal.IsMatch(plate)) throw new PlateException("Placa inválida!");
            }
        }

        private void ValidateEngineType(EngineType engineType)
        {
           if (!Enum.IsDefined(typeof(EngineType), engineType))
            {
                throw new DomainValidationException("Invalid engine type", nameof(engineType), nameof(Motorcycle));
            }
        }

        private void ValidateLastRevisionDate(DateTime lastRevisionDate)
        {
            if (lastRevisionDate > DateTime.Now.AddDays(1))
            {
                throw new DomainValidationException("Last revision date must be in the past", nameof(lastRevisionDate), nameof(Motorcycle));
            }
        }
    }
}
