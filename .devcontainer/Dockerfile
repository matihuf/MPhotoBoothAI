FROM ubuntu:22.04
ENV LC_ALL=C.UTF-8 LANG=C.UTF-8
RUN  apt-get update && apt-get install -y sudo wget git

# Dotnet Framework
# https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu
WORKDIR /tmp
RUN wget https://packages.microsoft.com/config/ubuntu/20.10/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb

RUN apt-get update && apt-get install -y dotnet-host
RUN apt-get update && apt-get install -y dotnet-sdk-8.0

# Make sure all emgu dependencies are in place
# http://www.emgu.com/wiki/index.php/Download_And_Installation#Getting_ready
WORKDIR /mnt/emgu_repo
RUN git clone https://github.com/emgucv/emgucv emgucv
WORKDIR /mnt/emgu_repo/emgucv
RUN git fetch origin 4.8.0
RUN git checkout 4.8.0
RUN git submodule update --init --recursive

RUN DEBIAN_FRONTEND=noninteractive TZ=Etc/UTC apt-get -y install tzdata
# install cmake for compiling open cv dependencies & emgu dependencies
RUN apt-get update && apt-get install -y build-essential cmake protobuf-compiler ffmpeg libgtk-3-dev libgstreamer1.0-dev libavcodec-dev libswscale-dev libavformat-dev libv4l-dev ocl-icd-dev freeglut3-dev libgeotiff-dev libusb-1.0-0-dev libdc1394-dev

RUN apt-get install -y build-essential libgtk-3-dev libgstreamer1.0-dev libavcodec-dev libswscale-dev libavformat-dev libdc1394-dev libv4l-dev cmake-curses-gui ocl-icd-dev freeglut3-dev libgeotiff-dev libusb-1.0-0-dev libvtk9-dev libfreetype-dev libharfbuzz-dev qtbase5-dev libeigen3-dev libgstreamer1.0-dev libgstreamer-plugins-base1.0-dev libgflags-dev libgoogle-glog-dev libatlas-base-dev liblapacke-dev libva-dev 

# this takes a long time
WORKDIR /mnt/emgu_repo/emgucv/platforms/ubuntu/22.04
RUN ./cmake_configure

WORKDIR /mnt/my_app_root
