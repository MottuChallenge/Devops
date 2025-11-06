using Moq;
using MottuChallenge.Application.DTOs.Request;
using MottuChallenge.Application.Repositories;
using MottuChallenge.Application.UseCases.Motorcycles;
using MottuChallenge.Domain.Entities;
using MottuChallenge.Domain.Enums;

namespace MottuChallenge.Test.Application.UseCases.Motorcycles;

public class UpdateMotorcycleUseCaseTests
{
        private UpdateMotorcycleUseCase CreateUseCase(Mock<IMotorcycleRepository> motoRepo, Mock<ISectorRepository> sectorRepo)
            => new UpdateMotorcycleUseCase(motoRepo.Object, sectorRepo.Object);

        private Motorcycle CreateMotorcycle()
            => new Motorcycle("old-model", (EngineType)0, "ABC-1234", DateTime.MinValue);

        private Sector CreateSectorInstance()
            => (Sector)Activator.CreateInstance(typeof(Sector), true);

        private Spot CreateSpot(double x = 0.1, double y = 0.1)
            => new Spot(x, y);

        [Fact]
        public async Task UpdateMotorcycleAsync_WhenMotorcycleNotFound_ThrowsKeyNotFoundException()
        {
            var motoId = Guid.NewGuid();
            var motoRepo = new Mock<IMotorcycleRepository>();
            var sectorRepo = new Mock<ISectorRepository>();

            motoRepo.Setup(r => r.GetByIdAsync(motoId)).ReturnsAsync((Motorcycle)null);

            var useCase = CreateUseCase(motoRepo, sectorRepo);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => useCase.UpdateMotorcycleAsync(motoId, new MotorcycleDto()));
        }

        [Fact]
        public async Task UpdateMotorcycleAsync_UpdatesFields_AndCallsMotorcycleRepositoryUpdate()
        {
            var motoId = Guid.NewGuid();
            var moto = CreateMotorcycle();
            var motoRepo = new Mock<IMotorcycleRepository>();
            var sectorRepo = new Mock<ISectorRepository>();

            motoRepo.Setup(r => r.GetByIdAsync(motoId)).ReturnsAsync(moto);
            motoRepo.Setup(r => r.UpdateAsync(It.IsAny<Motorcycle>())).Returns(Task.FromResult(moto)).Verifiable();

            var dto = new MotorcycleDto()
            {
                Model = "new-model",
                EngineType = 0,
                Plate = "ABC-1234",
                LastRevisionDate = DateTime.UtcNow
            };

            var useCase = CreateUseCase(motoRepo, sectorRepo);

            var result = await useCase.UpdateMotorcycleAsync(motoId, dto);

            Assert.Equal(dto.Model, result.Model);
            Assert.Equal(dto.EngineType, result.EngineType);
            Assert.Equal(dto.Plate, result.Plate);
            Assert.Equal(dto.LastRevisionDate, result.LastRevisionDate);
            motoRepo.Verify(r => r.UpdateAsync(It.IsAny<Motorcycle>()), Times.Once);
        }

        [Fact]
        public async Task UpdateMotorcycleAsync_RemovesMotorcycleFromOldSpot_AndUpdatesOldSector()
        {
            var motoId = Guid.NewGuid();

            var oldSpot = CreateSpot();
            var moto = CreateMotorcycle();
            moto.SetSpot(oldSpot);

            var oldSector = CreateSectorInstance();

            var motoRepo = new Mock<IMotorcycleRepository>();
            var sectorRepo = new Mock<ISectorRepository>();

            motoRepo.Setup(r => r.GetByIdAsync(motoId)).ReturnsAsync(moto);
            sectorRepo.Setup(r => r.GetSectorBySpotId(oldSpot.SpotId)).ReturnsAsync(oldSector);
            sectorRepo.Setup(r => r.UpdateAsync(oldSector)).Returns(Task.FromResult(oldSector)).Verifiable();
            motoRepo.Setup(r => r.UpdateAsync(It.IsAny<Motorcycle>())).Returns(Task.FromResult(moto));

            var dto = new MotorcycleDto { Model = "keep", EngineType = (int)((EngineType)0), Plate = "ABC-1234", LastRevisionDate = DateTime.UtcNow };

            var useCase = CreateUseCase(motoRepo, sectorRepo);

            await useCase.UpdateMotorcycleAsync(motoId, dto);

            sectorRepo.Verify(r => r.GetSectorBySpotId(oldSpot.SpotId), Times.Once);
            sectorRepo.Verify(r => r.UpdateAsync(oldSector), Times.Once);
            motoRepo.Verify(r => r.UpdateAsync(It.IsAny<Motorcycle>()), Times.Once);
        }

        [Fact]
        public async Task UpdateMotorcycleAsync_AssignsMotorcycleToNewSpot_AndUpdatesNewSector()
        {
            var motoId = Guid.NewGuid();
            var moto = CreateMotorcycle();

            var newSpot = CreateSpot();
            var newSector = CreateSectorInstance();
            var spots = new List<Spot> { newSpot };
            newSector.AddSpots(spots);

            var motoRepo = new Mock<IMotorcycleRepository>();
            var sectorRepo = new Mock<ISectorRepository>();

            motoRepo.Setup(r => r.GetByIdAsync(motoId)).ReturnsAsync(moto);
            sectorRepo.Setup(r => r.GetSectorBySpotId(newSpot.SpotId)).ReturnsAsync(newSector);
            sectorRepo.Setup(r => r.UpdateAsync(newSector)).Returns(Task.FromResult(newSector)).Verifiable();
            motoRepo.Setup(r => r.UpdateAsync(It.IsAny<Motorcycle>())).Returns(Task.FromResult(moto));

            var dto = new MotorcycleDto
            {
                Model = "m",
                EngineType = (int)((EngineType)0),
                Plate = "ABC-1234",
                LastRevisionDate = DateTime.UtcNow,
                SpotId = newSpot.SpotId
            };

            var useCase = CreateUseCase(motoRepo, sectorRepo);

            var result = await useCase.UpdateMotorcycleAsync(motoId, dto);

            sectorRepo.Verify(r => r.GetSectorBySpotId(newSpot.SpotId), Times.Once);
            sectorRepo.Verify(r => r.UpdateAsync(newSector), Times.Once);
            motoRepo.Verify(r => r.UpdateAsync(It.IsAny<Motorcycle>()), Times.Once);
            Assert.Equal(newSpot.SpotId, result.SpotId);
        }

        [Fact]
        public async Task UpdateMotorcycleAsync_WhenTargetSpotNotFound_ThrowsKeyNotFoundException()
        {
            var motoId = Guid.NewGuid();
            var moto = CreateMotorcycle();

            var motoRepo = new Mock<IMotorcycleRepository>();
            var sectorRepo = new Mock<ISectorRepository>();

            motoRepo.Setup(r => r.GetByIdAsync(motoId)).ReturnsAsync((Motorcycle)null!);

            var targetSpot = CreateSpot();
            var sectorWithoutSpot = CreateSectorInstance();

            sectorRepo.Setup(r => r.GetSectorBySpotId(targetSpot.SpotId)).ReturnsAsync(sectorWithoutSpot);

            var dto = new MotorcycleDto
            {
                Model = "m",
                EngineType = (int)((EngineType)0),
                Plate = "ABC-1234",
                LastRevisionDate = DateTime.UtcNow,
                SpotId = targetSpot.SpotId
            };

            var useCase = CreateUseCase(motoRepo, sectorRepo);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => useCase.UpdateMotorcycleAsync(motoId, dto));
        }
}