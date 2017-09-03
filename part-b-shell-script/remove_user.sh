#!/bin/bash
SUCCESS=0
username=$1
grep -q "$username" /etc/passwd
if [ $? -eq $SUCCESS ] 
	then	  
		if [ $? -eq 0 ] 
			then
			userdel $username
			rm -r /home/$username
			echo "Successfully deleted the $username and the directories inside"
		else
			echo "fail"
		fi
else 
echo "The $username does not exists"
fi
