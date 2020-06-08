using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BOIVR
{
    public class Room : MonoBehaviour
    {
        public enum RoomType
        {
            Regular,
            Start,
            Boss,
            Treasure,
            Teleporter
        }

        public RoomType roomType;
        public TextMeshPro text;
        public int number;

        public delegate void RoomDelegate();

        public RoomDelegate RoomStartEvent;
        public RoomDelegate RoomEndEvent;

        public List<Door> doors;

        public static Room activeRoom;

        public bool isCompleted = false;
        public bool hasStarted;

        public List<Transform> corners;
        public RoomManager roomManager;

        // Start is called before the first frame update
        void Start()
        {
            if (doors.Count > 0)
            {
                foreach (Door door in doors)
                {
                    RoomStartEvent += door.LockDoor;
                    RoomEndEvent += door.OpenDoor;

                    door.rooms.Add(this);
                }
            }

        }

        public void RoomClear()
        {
            isCompleted = true;
            RoomEndEvent?.Invoke();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (isCompleted)
            {
                return;
            }

            if (hasStarted)
            {
                return;
            }

            if (other.gameObject.GetComponentInParent<Body>())
            {
                RoomStartEvent?.Invoke();
                hasStarted = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Enemy enemy = other.gameObject.GetComponentInParent<Enemy>();

            if (enemy)
            {
                enemy.Kill();
            }

        }
    }

}