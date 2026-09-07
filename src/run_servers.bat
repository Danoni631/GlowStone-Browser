@echo off
title GlowStone servers
color 50

cd homepage
start homepage_server.py
cd ".."
cd weather
start weather_server.py
cd ".."
echo All servers are running