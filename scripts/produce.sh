#!/usr/bin/bash
stream=('one' 'two' 'three' 'four' 'five')

for item in "${stream[@]}"
do
	echo "$item"
	sleep 5
done

