using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Patterns
{

    public abstract class Repository
    {
        public abstract void AddObject(DataObject dataObject);
        public abstract void CopyObject(DataObject dataObject);
        public abstract void RemoveObject(DataObject dataObject);

        public void SaveChanges()
        {
            Console.WriteLine("Changes were saved.");
        }

    }

    public class ClientRepository : Repository
    {
        public override void AddObject(DataObject dataObject)
        {
            // Do repository specific work
            dataObject.Register();
        }

        public override void CopyObject(DataObject dataObject)
        {
            // Do repository specific work
            dataObject.Copy();
        }

        public override void RemoveObject(DataObject dataObject)
        {
            // Do repository specific work
            dataObject.Delete();
        }
    }

    public class ProductRepository : Repository
    {
        public override void AddObject(DataObject dataObject)
        {
            // Do repository specific work
            dataObject.Register();
        }

        public override void CopyObject(DataObject dataObject)
        {
            // Do repository specific work
            dataObject.Copy();
        }

        public override void RemoveObject(DataObject dataObject)
        {
            // Do repository specific work
            dataObject.Delete();
        }
    }

    public abstract class DataObject
    {
        public abstract void Register();
        public abstract DataObject Copy();
        public abstract void Delete();
    }

    public class ClientDataObject : DataObject
    {
        public override void Register()
        {
            Console.WriteLine("ClientDataObject was registered");
        }

        public override DataObject Copy()
        {
            Console.WriteLine("ClientDataObject was copied");
            return new ClientDataObject();
        }

        public override void Delete()
        {
            Console.WriteLine("ClientDataObject was deleted");
        }
    }

    public class ProductDataObject : DataObject
    {
        public override void Register()
        {
            Console.WriteLine("ProductDataObject was registered");
        }

        public override DataObject Copy()
        {
            Console.WriteLine("ProductDataObject was copied");
            return new ProductDataObject();
        }

        public override void Delete()
        {
            Console.WriteLine("ProductDataObject was deleted");
        }
    }

    public class BridgePattern
    {
        public static void Bridge()
        {
            var clientRepository = new ClientRepository();
            var productRepository = new ProductRepository();

            var clientDataObject = new ClientDataObject();
            clientRepository.AddObject(clientDataObject);
            clientRepository.SaveChanges();

            clientRepository.CopyObject(clientDataObject);

            clientRepository.RemoveObject(clientDataObject);
            clientRepository.SaveChanges();

            var productDataObject = new ProductDataObject();
            productRepository.AddObject(productDataObject);
            clientRepository.SaveChanges();

            productRepository.CopyObject(productDataObject);

            productRepository.RemoveObject(productDataObject);
            productRepository.SaveChanges();

            Console.ReadKey();
        }
    }
}
