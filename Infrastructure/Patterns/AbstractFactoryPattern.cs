using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Patterns
{
    /// <summary>
    /// Abstract Factory Pattern
    /// Create instances of classes belonging to different families
    /// </summary>
    public abstract class CarFactory
    {
        public abstract SportsCar CreateSportsCar();
        public abstract FamilyCar CreateFamilyCar();
    }

    public class MercedesFactory : CarFactory
    {
        public override SportsCar CreateSportsCar()
        {
            return new MercedesSportsCar();
        }

        public override FamilyCar CreateFamilyCar()
        {
            return new MercedesFamilyCar();
        }
    }

    public class AudiFactory : CarFactory
    {
        public override SportsCar CreateSportsCar()
        {
            return new AudiSportsCar();
        }

        public override FamilyCar CreateFamilyCar()
        {
            return new AudiFamilyCar();
        }
    }

    public abstract class SportsCar
    {

    }

    public abstract class FamilyCar
    {
        public abstract void Speed(SportsCar abstractSportsCar);
    }

    public class AudiSportsCar : SportsCar
    {

    }

    public class AudiFamilyCar : FamilyCar
    {
        public override void Speed(SportsCar abstractSportsCar)
        {
            Console.WriteLine(GetType().Name + " is slower than "
                + abstractSportsCar.GetType().Name);
        }
    }
    public class MercedesSportsCar : SportsCar
    {

    }

    public class MercedesFamilyCar : FamilyCar
    {
        public override void Speed(SportsCar abstractSportsCar)
        {
            Console.WriteLine(GetType().Name + " is slower than "
                + abstractSportsCar.GetType().Name);
        }
    }

    public class Driver
    {
        private SportsCar _sportsCar;
        private FamilyCar _familyCar;

        public Driver(CarFactory carFactory)
        {
            _sportsCar = carFactory.CreateSportsCar();
            _familyCar = carFactory.CreateFamilyCar();
        }

        public void CompareSpeed()
        {
            _familyCar.Speed(_sportsCar);
        }
    }

    public class GenericFactory<T>
        where T : new()
    {
        public T CreateObject()
        {
            return new T();
        }
    }

    public class ConsoleApplication
    {
        public void AbstractFactory()
        {
            var audiFactory = new AudiFactory();
            Driver driver = new Driver(audiFactory);
            driver.CompareSpeed();

            var factory = new GenericFactory<MercedesSportsCar>();
            var mercedesSportsCar = factory.CreateObject();

            Console.WriteLine(mercedesSportsCar.GetType().ToString());
            Console.ReadKey();
        }
    }
}
