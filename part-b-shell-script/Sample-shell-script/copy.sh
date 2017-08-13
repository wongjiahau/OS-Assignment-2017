#!/bin/sh
for i in 6 6.1 6.2 6.3 6.4 6.5 6.6 6.7 6.7.2 6.7.3 6.7.5 6.7.6 7.0 7.1 7.2 7.3 7.4 7.5
do 
  s="$HOME/Desktop/TTAP Git/Time Table Arranging Program v$i/Time Table Arranging Program/bin/Debug/."
  Copying files "Navigating to $s"
  s2="$HOME/Desktop/TTAP Git/TTAP-Releases/TTAP v$i"
  mkdir "$s2"
  cp -R "$s" "$s2"
done
