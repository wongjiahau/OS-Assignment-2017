# Algorithm
## Main function 
1. Create and start thread "Loader"
2. Create and start thread "Rotator"
3. Create and start thread "Picker"
4. Wait until AllItemsDelivered event is signaled
5. When all items is delivered, kill all the threads 
6. End

## Loader 
1. Wait until LoadPlaceEmpty event is signaled
2. Wait for RotatorMutex 
3. Stop if number_of_item_loaded equals number_of_item_to_be_delivered 
4. Load item, increment number_of_item_loaded 
5. Reset LoadPlaceEmpty event
6. Signal Loaded event
7. Release RotatorMutex

## Picker
1. Wait until PickPlaceFull event is signaled
2. Wait for RotatorMutex 
3. Signal AllItemsDelivered event if number_of_item_picked equals number_of_item_to_be_delivered 
4. Pick item, increment number_of_item_picked
5. Reset LoadPlaceFull event
6. Signal Picked event
7. Release RotatorMutex

## Rotator
1. Wait until Loaded event is signaled
2. Wait until Picked event is signaled
3. Wait for Rotator mutex
4. Stop if number_of_item_picked equals number_of_item_to_be_delivered
5. Rotate
6. Reset Picked event
7. Reset Loaded event
8. Signal PickPlaceFull event
9. Signal LoadPlaceEmpty event
10. Release RotatorMutex
