using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    public abstract class GameObject
    {

        public GameObject() { }
        public GameObject(double xcoor, double ycoor) {
            Xcoor = xcoor;
            Ycoor = ycoor;
        }
        public double Xcoor { get; set; }
        public double Ycoor { get; set; }
        public string Type { get; set; }
        public abstract string Serialize();
        public virtual void Deserialize(string info)
        {
            Xcoor = Convert.ToDouble(JsonUtils.ExtractValue(info, "xcoor"));
            Ycoor = Convert.ToDouble(JsonUtils.ExtractValue(info, "ycoor"));
        }

        public static T FromJsonFactory<T>(string json) where T : GameObject, new()
        {
            //https://stackoverflow.com/questions/14696904/cannot-create-an-instance-of-the-variable-type-item-because-it-does-not-have-t
            T gameobject = new T();
            gameobject.Deserialize(json);
            return gameobject;
        }
    }
}
