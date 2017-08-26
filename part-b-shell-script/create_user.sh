create_user(){
 USER=$1
 shift;
echo "I will create you a file called ${username}_file"
mkdir -p /$username/{guildeline,backup}
cp Readme.txt /$username/guildeline
chmod 777 /$username/guildeline/Readme.txt
echo "The end"
}

create_user abc


