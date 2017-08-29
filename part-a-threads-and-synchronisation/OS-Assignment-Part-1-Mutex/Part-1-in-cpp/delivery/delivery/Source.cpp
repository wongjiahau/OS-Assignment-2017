#include <iostream>
#include <string>
#include <thread>
#include <mutex>
#include <condition_variable>

using namespace std;
mutex m;
condition_variable cv;
string Data;
bool ready = false;
bool processed = false;

int number_of_items_to_be_delivered;
void worker_thread() {
	// Wait until main() sends Data
	unique_lock<mutex> lk(m);
	cv.wait(lk , [] { return ready; });

	// after the wait, we own the lock.
	cout << "Worker thread is processing Data\n";
	Data += " after processing";

	// Send Data back to main()
	processed = true;
	cout << "Worker thread signals Data processing completed\n";

	// Manual unlocking is done before notifying, to avoid waking up
	// the waiting thread only to block again (see notify_one for details)
	lk.unlock();
	cv.notify_one();
}

void ValidateUserInput(int argc , char* argv[]) {
	if(argc < 2) {
		cout << "Error : Please pass in an integer argument that specify the number of items to be delivered.";
	}
	else if(argc > 2) {
		cout << "Only one argument is needed.";
	}
	else {
		try {
			number_of_items_to_be_delivered = stoi(argv[1]);
			if(number_of_items_to_be_delivered < 1) {
				cout << "Error : Argument must be greater than 1.";
				return;
			}
			string message = "Number of items to be delivered is set to ";
			message.append(std::to_string(number_of_items_to_be_delivered));
			cout << message;
		}
		catch(exception ex) {
			cout << "Error : Argument must be integer.";
		}
	}
}


int main(int argc , char *argv[]) {
	ValidateUserInput(argc , argv);
	return 0;
	thread worker(worker_thread);

	Data = "Example Data";
	// send Data to the worker thread
	{
		lock_guard<mutex> lk(m);
		ready = true;
		cout << "main() signals Data ready for processing\n";
	}
	cv.notify_one();

	// wait for the worker
	{
		unique_lock<mutex> lk(m);
		cv.wait(lk , [] { return processed; });
	}
	cout << "Back in main(), Data = " << Data << '\n';

	worker.join();
	getchar();
}
