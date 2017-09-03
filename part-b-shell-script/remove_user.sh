#!/bin/bash
ROOT_UID=0
SUCCESS=0
E_USEREXISTS=70
username=$1
grep -q "$username" /etc/passwd
if [ $? -eq $SUCCESS ] 
	then	  
		if [ $? -eq 0 ] 
			then
			userdel $username
			rm -r /home/$username
			echo "Successfully deleted the user and the directories inside"
		else
			echo "fail"
		fi
else 
echo "The $username does not exists"
fi
