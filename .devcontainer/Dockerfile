FROM ubuntu:latest

RUN apt-get update && apt-get install -y dotnet-sdk-8.0

RUN apt-get install -y build-essential libgtk-3-dev libgstreamer1.0-dev \
libavcodec-dev libswscale-dev libavformat-dev libdc1394-dev libv4l-dev \
cmake-curses-gui ocl-icd-dev freeglut3-dev libgeotiff-dev libusb-1.0-0-dev \
libvtk9-dev libfreetype-dev libharfbuzz-dev qtbase5-dev libeigen3-dev \
libgstreamer1.0-dev libgstreamer-plugins-base1.0-dev libgflags-dev \
libgoogle-glog-dev liblapacke-dev libva-dev

WORKDIR /mnt/my_app_root
