using System;
using System.Collections.Generic;

namespace Commercial_Controller
{
    public class Battery
    {
        public int ID;
        public string status;
        public int amountOfColumns;
        public int amountOfFloors;
        public int amountOfBasements;
        public int amountOfElevatorPerColumn;
        public List<Column> columnsList;
        public List<FloorRequestButton> floorRequestButtonList;
        public int columnID = 1;
        public int elevatorID = 1;
        public int floorRequestButtonID = 1;
        public int callButtonId = 1;    


        
        public Battery(int _id, int _amountOfColumns, int _amountOfFloors, int _amountOfBasements, int _amountOfElevatorPerColumn)
        {
            ID = _id;
            status = "online";
            amountOfColumns = _amountOfColumns;
            amountOfFloors = _amountOfFloors;
            amountOfBasements = _amountOfBasements;
            amountOfElevatorPerColumn = _amountOfElevatorPerColumn;
            columnsList = new List<Column>();
            floorRequestButtonList = new List<FloorRequestButton>();

            createFloorRequestButtons(_amountOfFloors);
            createColumns(_amountOfColumns, _amountOfFloors, _amountOfElevatorPerColumn);


            if (_amountOfBasements > 0){
            createBasementFloorRequestButtons(_amountOfBasements);
            createBasementColumn(_amountOfBasements, _amountOfElevatorPerColumn);
            }

            // createFloorRequestButtons(_amountOfFloors);
            // createColumns(_amountOfColumns, _amountOfFloors, _amountOfElevatorPerColumn);
        }

        public void createBasementColumn(int _amountOfBasements, int _amountOfElevatorPerColumn)
        {

            List<int> servedFloors = new List<int>();
            int floor = -1;
            for(int i = 0; i < _amountOfBasements; i++){
                servedFloors.Add(floor);
                floor--;
            }
            Column column = new Column(columnID, "online", _amountOfBasements, _amountOfElevatorPerColumn, servedFloors, true);
            columnsList.Add(column);
            columnID++;
        }

        public void createColumns(int _amountOfColumns, int _amountOfFloors, int _amountOfElevatorPerColumn)
        {
            int _amountOfFloorsPerColumn = (int)Math.Ceiling((double)(_amountOfFloors / _amountOfColumns));
            // int _amountOfFloorsPerColumn = 20;
            int floor = 1;

            for(int i = 0; i < _amountOfColumns; i++){
                List<int> servedFloors = new List<int>();
                for(int k = 0; k < _amountOfFloorsPerColumn; k++){
                    if(floor <= _amountOfFloors){
                        servedFloors.Add(floor);
                        floor++;
                    }
                }
                Column column = new Column(columnID, "online", _amountOfFloors, _amountOfElevatorPerColumn, servedFloors, false);
                columnsList.Add(column);
                columnID++;
            }
        }

        public void createFloorRequestButtons(int _amountOfFloors){
            int buttonFloor = 1;
            for(int i = 0; i < _amountOfFloors; i++){
                FloorRequestButton floorRequestButton = new FloorRequestButton(1, "off", buttonFloor, "up");
                floorRequestButtonList.Add(floorRequestButton);
                buttonFloor++;
                floorRequestButtonID++;
            }
        }

        public void createBasementFloorRequestButtons(int _amountOfBasements){
            int buttonFloor = -1;
            for(int i = 0; i < _amountOfBasements; i++){
                FloorRequestButton floorRequestButton  = new FloorRequestButton(1, "off", buttonFloor, "down");
                floorRequestButtonList.Add(floorRequestButton);
                buttonFloor--;
                floorRequestButtonID++;
            }
        }


        public Column findBestColumn(int _requestedFloor)
        {

            foreach(Column column in columnsList)
            {
                if(column.servedFloorsList.Contains(_requestedFloor)){
                    return column;
                }
            }

            return null;
        }
        //Simulate when a user press a button at the lobby
        public (Column, Elevator) assignElevator(int _requestedFloor, string _direction)
        {
            Column column = findBestColumn(_requestedFloor);
            Elevator elevator = column.findElevator(1, _direction);
            elevator.addNewRequest(1);
            elevator.move();

            elevator.addNewRequest(_requestedFloor);
            elevator.move();

            return (column, elevator);

        }
    }
}

