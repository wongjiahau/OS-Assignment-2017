#!/bin/bash
#!OOT_UID=0
E_USEREXISTS=70
username=$1
grep -q "$username" /etc/passwd  
	if [ $? -eq $SUCCESS ] 
	then	
	echo "User $username does already exist."
  	echo "please chose another username."
	exit $E_USEREXISTS
	fi
useradd  -d /home/"$username" -m -g users -s /bin/bash "$username"
echo "Account $username is setup successfully."
sleep 2
if [ $? -eq 0 ] 	
then
echo "User had been added to system!" 		
echo "and now creating directory for the $username" 
sleep 1
mkdir /home/$username/guildelines
mkdir /home/$username/backup
cp Readme.txt /home/$username/guildelines
chown $username /home/$username
chown $username /home/$username/guildelines
chown $username /home/$username/backup
chown $username /home/$username/guildelines/Readme.txt
echo "Finish added"
else 
echo "fail"
fi
