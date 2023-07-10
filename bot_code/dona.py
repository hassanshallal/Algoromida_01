import sys
sys.path.insert(1, 'dona_assets')

from flask import request
from flask import jsonify
import nltk
from nltk import sent_tokenize

from dona_utils import *
from respond import *

respond_inst = Respond()

# Model function


def internalmodel(this_user_info, query, this_user_interactions, platform):
    num_records = len(this_user_interactions)

    print(this_user_info)
    print("This user has " + str(num_records) + " previous interactions.")

    if this_user_info == None and platform == "Facebook":
        return None, None, None

    elif this_user_info != None:
        # send and receive from the model
        this_response, this_awareness, this_statefulness, best_match = respond_inst.cognify(
            query, this_user_info, this_user_interactions)
    print("awareness is: " + this_awareness + ", statefulness is: " + str(this_statefulness) +
          ", response is: " + this_response + ", best match is: " + str(best_match))
    return this_awareness, this_statefulness, this_response, best_match


def dona():
    text = request.json['text']
    sender_info = request.json["senderInfo"].split(",")

    prev_interactions = request.json["prevInteactions"]
    if prev_interactions != "":
        prev_interactions = request.json["prevInteactions"].split("_")
        prev_interactions = [tuple(x.split("<>"))
                             for x in prev_interactions]
    else:
        prev_interactions = []

    query = process_query(text)
    texts = sent_tokenize(query)
    for text in texts:
        print(text)
        this_awareness, this_statefulness, this_response, best_match = internalmodel(
            sender_info, text, prev_interactions, "Algoromida")
        print(this_response)
        this_response = process_response(str(this_response))
        payload = jsonify({'BotResponse': this_response, 'BotAwareness': this_awareness,
                           'BotStatefulness': str(this_statefulness)})
        return payload
