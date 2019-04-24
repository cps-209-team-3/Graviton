//This file holds the GameObject class which represents all objects in the game.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    //This is an abstract class which represents all of the objects in the game.
    public abstract class GameObject
    {

        public GameObject() { }
        public GameObject(double xcoor, double ycoor) {
            Xcoor = xcoor;
            Ycoor = ycoor;
        }
        //The coordinates of the object
        public double Xcoor { get; set; }
        public double Ycoor { get; set; }
        //A string representing the type of the object
        public string Type { get; set; }
        public abstract string Serialize();

        //This method deserializes a string to set the coordinates
        public virtual void Deserialize(string info)
        {
            Xcoor = Convert.ToDouble(JsonUtils.ExtractValue(info, "xcoor"));
            Ycoor = Convert.ToDouble(JsonUtils.ExtractValue(info, "ycoor"));
        }

        //This method takes a string and returns a deserialized GameObject
        public static T FromJsonFactory<T>(string json) where T : GameObject, new()
        {
            //https://stackoverflow.com/questions/14696904/cannot-create-an-instance-of-the-variable-type-item-because-it-does-not-have-t
            T gameobject = new T();
            gameobject.Deserialize(json);
            return gameobject;
        }
    }
}
