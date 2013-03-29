#! /usr/bin/python
# -*- coding: utf-8 -*-
# python 3.2

import hashlib
import base64
import time
import random

class AzDG:
	"""docstring for AzDG"""
	 __all__ = ["Encrypt", "Decrypt"]
    cipher = 'Private key'
	charset = 'utf-8'
	
	def __init__(self, cipher = None):
		if cipher != None:
			self.cipher = cipher

	def cipherEncode(self, sourceText):	
		cipherHash = hashlib.md5(self.cipher.encode(self.charset)).hexdigest().encode(self.charset)		
		encodeText = bytearray()
		for i in range(len(sourceText)):			
			encodeText.append(sourceText[i] ^ cipherHash[i%32])	
		return encodeText.decode('ISO-8859-1')
				
	def Encrypt(self, sourceText, charset = 'utf-8'):
		if charset != self.charset:
			self.charset = charset
		sourceText = sourceText.encode(self.charset)		
		noise = hashlib.md5(str(time.time()).encode(self.charset)).hexdigest().encode(self.charset)		
		encodeText = bytearray()
		for i in range(len(sourceText)):			
			encodeText.append(noise[i%32])	
			encodeText.append(sourceText[i] ^ noise[i%32])		
		return base64.b64encode(self.cipherEncode(encodeText).encode('ISO-8859-1')).decode(self.charset)
	
	def Decrypt(self, sourceText, charset = 'utf-8'):
		if charset != self.charset:
			self.charset = charset
		decodeSourceText = self.cipherEncode(base64.b64decode(sourceText.encode(self.charset)))		
		textLength = len(decodeSourceText)
		decodeText = bytearray()
		i = 0			
		while i < textLength:	
			decodeText.append(ord(decodeSourceText[i]) ^ ord(decodeSourceText[i+1]))	
			i += 2
		return decodeText.decode(self.charset)    


