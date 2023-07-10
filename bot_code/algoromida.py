# We need modules only needed for algoromida
from flask import Flask


# Get other chatbots
from dona import *
from thermobot import *

# initiate container of chatbots
algoromida = Flask(__name__)

# Import and decorate dona and thermobot from thermogena.py
dona = algoromida.route("/Dona", methods=['POST'])(dona)
thermobot = algoromida.route("/Thermobot", methods=['POST'])(thermobot)


if __name__ == "__main__":
    algoromida.run()
