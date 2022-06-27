using System;
using System.Collections.Generic;

namespace Commercial_Controller
{
    public class Column
    {
        public int ID;
        public string status;
        public int amountOfFloors;
        public int amountOfElevators;
        public List<Elevator> elevatorsList;
        public List<CallButton> callButtonsList;
        public List<int> servedFloorsList;
        public bool isBasement;
        public int elevatorID;


        public Column(int _id, string _status, int _amountOfFloors, int _amountOfElevators, List<int> _servedFloors, bool _isBasement)
        {
            ID = _id;
            status = _status;
            amountOfFloors = _amountOfFloors;
            amountOfElevators = _amountOfElevators;
            servedFloorsList = _servedFloors;
            isBasement = _isBasement;
            elevatorsList = new List<Elevator>();
            callButtonsList = new List<CallButton>();
            servedFloorsList = _servedFloors;
            
            createElevators(_amountOfFloors, _amountOfElevators);
            createCallButtons(_amountOfFloors, _isBasement);
        }


        public void createCallButtons(int _amountOfFloors, bool _isBasement)
        {
            if(_isBasement){
                int buttonFloor = -1;
                for(int i = 0; i < _amountOfFloors; i++){
                    CallButton callButton = new CallButton(i+1, "off", buttonFloor, "up");
                    callButtonsList.Add(callButton);
                    buttonFloor--;
                }
            }
            else{
                int buttonFloor = 1;
                for(int i = 0; i < _amountOfFloors; i++){
                    CallButton callButton = new CallButton(i+1, "off", buttonFloor, "down");
                    callButtonsList.Add(callButton);
                    buttonFloor++;
                }
            }

        }

        public void createElevators(int _amountOfFloors, int _amountOfElevators)
        {
            for(int i = 0; i < _amountOfElevators; i++){
                Elevator elevator = new Elevator(i+1, "idle", _amountOfFloors, 1);
                elevatorsList.Add(elevator);
            }
        }

        //Simulate when a user press a button on a floor to go back to the first floor
        public Elevator requestElevator(int userPosition, string direction)
        {
           Elevator elevator = findElevator(userPosition, direction);
           elevator.addNewRequest(userPosition);
           elevator.move();

           elevator.addNewRequest(1);
           elevator.move();
           return elevator;
        }

        public Elevator findElevator(int requestedFloor, string _requestedDirection){
            BestElevatorInformations bestElevatorInformations = new BestElevatorInformations{
                bestScore = 6,
                referenceGap = 10000000,
                bestElevator = null
            };

            if(requestedFloor == 1){
                foreach(Elevator elevator in elevatorsList){
                    if(1 == elevator.currentFloor && elevator.status == "stopped"){
                        bestElevatorInformations = checkIfElevatorIsBetter(1, elevator, bestElevatorInformations, requestedFloor);
                    }
                    else if(1 == elevator.currentFloor && elevator.status == "idle"){
                        bestElevatorInformations = checkIfElevatorIsBetter(2, elevator, bestElevatorInformations, requestedFloor);
                    }
                    else if(1 > elevator.currentFloor && elevator.direction == "up"){
                        bestElevatorInformations = checkIfElevatorIsBetter(3, elevator, bestElevatorInformations, requestedFloor);
                    }
                    else if(1 < elevator.currentFloor && elevator.direction == "down"){
                        bestElevatorInformations = checkIfElevatorIsBetter(3, elevator, bestElevatorInformations, requestedFloor);
                    }
                    else if(elevator.status == "idle"){
                        bestElevatorInformations = checkIfElevatorIsBetter(4, elevator, bestElevatorInformations, requestedFloor);
                    }
                    else{
                        bestElevatorInformations = checkIfElevatorIsBetter(5, elevator, bestElevatorInformations, requestedFloor);
                    }
                    // bestElevator = bestElevatorInformations.bestElevator;
                    // bestScore = bestElevatorInformations.bestScore;
                    // referenceGap = bestElevatorInformations.referenceGap;
                }
            }
            else{
                foreach(Elevator elevator in elevatorsList){
                    if(requestedFloor == elevator.currentFloor && elevator.status == "stopped" && _requestedDirection == elevator.direction){
                        bestElevatorInformations = checkIfElevatorIsBetter(1, elevator, bestElevatorInformations, requestedFloor);
                    }
                    else if(requestedFloor > elevator.currentFloor && elevator.direction == "up" && _requestedDirection ==  "up"){
                        bestElevatorInformations = checkIfElevatorIsBetter(2, elevator, bestElevatorInformations, requestedFloor);
                    }
                    else if(requestedFloor < elevator.currentFloor && elevator.direction == "down" && _requestedDirection == "down"){
                        bestElevatorInformations = checkIfElevatorIsBetter(2, elevator, bestElevatorInformations, requestedFloor);
                    }
                    else if(elevator.status == "idle"){
                        bestElevatorInformations = checkIfElevatorIsBetter(4, elevator, bestElevatorInformations, requestedFloor);
                    }
                    else{
                        bestElevatorInformations = checkIfElevatorIsBetter(5, elevator, bestElevatorInformations, requestedFloor);
                    }
                    // bestElevator = bestElevatorInformations.bestElevator;
                    // bestScore = bestElevatorInformations.bestScore;
                    // referenceGap = bestElevatorInformations.referenceGap;
                
                }

            }
            return bestElevatorInformations.bestElevator;
        }   

        public BestElevatorInformations checkIfElevatorIsBetter(int scoreToCheck, Elevator newElevator, BestElevatorInformations bestElevatorInformations, int floor){
            if (scoreToCheck < bestElevatorInformations.bestScore){
            bestElevatorInformations.bestScore = scoreToCheck;
            bestElevatorInformations.bestElevator = newElevator;
            bestElevatorInformations.referenceGap = Math.Abs(newElevator.currentFloor - floor);
            }
            else if(bestElevatorInformations.bestScore == scoreToCheck){
                int gap = Math.Abs(newElevator.currentFloor - floor);
                if (bestElevatorInformations.referenceGap > gap){
                    bestElevatorInformations.bestElevator = newElevator;
                    bestElevatorInformations.referenceGap = gap;
                };
            };
            return bestElevatorInformations;
        }


            








    }
}