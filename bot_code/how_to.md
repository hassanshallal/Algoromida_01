#### This Algoromida folder is designed to have a .py and an _assets folder for each chatbot
## dona.py and dona_assets folder are whatever is needed for Dona, etc
## This is just a start as far as streamlining the process of creating different chatbots with different logic nehind them.




#### There are two parts: 
1. Creating an environment.
2. Running training and inference.

==========================================================================

#### Creating an environment
                      
1. Please open a terminal and make sure the current directory is Algoromida_02

2. On the terminal, type the following:
```bash
conda create --name dona python=3.7
conda activate dona
python3.7 -m pip install -r requirements.txt

pip install --pre torch torchvision -f https://download.pytorch.org/whl/nightly/cu101/torch_nightly.html
```
Please note that installing the packages in the requirements.txt may take sometime.

In order to deactivate the environment:
```bash
deactivate
```

3. Run corpora.py in order to download the stopwords using nltk.download(). After running this script, please wait for a few seconds for the nltk.download() window to open. Navigate to corpora and select stopwords and hit download. On the terminal:
```bash
python3 corpora.py
```

5. Download Spacy model used for preprocessing and cleaning.
```bash
python3 -m spacy download en_core_web_md
```
