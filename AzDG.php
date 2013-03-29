<?php
class AzDG {

    private $strPrivateKey = 'Private key';
    private $strDefaultCharset = 'big5';
    private $strTargetCharset = '';
    private $strSourceText = '';
    private $intSourceLength = 0;

    function __construct() {
    }

    private function private_key_crypt($argStrSourceText) {
        $strEncryptKey = md5($this->strPrivateKey);
        $intCRCLength = 0;
        $strReturn = "";
        $intSourceLength = strlen($argStrSourceText);
        for ($i = 0; $i < $intSourceLength; ++$i) {
            $strReturn .= ($argStrSourceText[$i] ^ $strEncryptKey[$i%32]); 
        }
        return $strReturn;
    } 

    public function Encrypt($argStrSourceText, $argStrPrivateKey = null ,$argStrCharset = null) {
        $this->strSourceText = $argStrSourceText;
        $this->intSourceLength = strlen($this->strSourceText);
        if (!is_null($argStrPrivateKey)) {
            $this->strPrivateKey = $argStrPrivateKey;
        }
        if (!is_null($argStrCharset) AND !empty($argStrCharset)) {
            $this->strTargetCharset = $argStrCharset;
            if (!$this->convert_encoding()) {
                die('編碼轉換失敗');
            } 
        }
        $strCRCKey = md5(microtime());
        $intCRCLength = 0;
        $strTmp = "";
        for ($i = 0; $i < $this->intSourceLength; ++$i) {
           $strTmp .= $strCRCKey[$intCRCLength] . ($this->strSourceText[$i] ^ $strCRCKey[$i%32]);
        }
        return base64_encode($this->private_key_crypt($strTmp)); 
    }

    public function Decrypt($argStrSourceText,$argStrPrivateKey = null ,$argStrCharset = null) { 
        if (!is_null($argStrPrivateKey)) {
            $this->strPrivateKey = $argStrPrivateKey;
        }

        if (!is_null($argStrCharset) AND !empty($argStrCharset)) {
            $this->strTargetCharset = $argStrCharset;
            if (!$this->convert_encoding()){
                die('編碼轉換失敗');
            } 
        }

        $this->strSourceText = $this->private_key_crypt(base64_decode($argStrSourceText));
        $this->intSourceLength = strlen($this->strSourceText);
        $strReturn = '';

        for ($i = 0; $i < $this->intSourceLength; ++$i) {
            $strReturn .= ($this->strSourceText[$i++] ^ $this->strSourceText[$i]);
        }
        return $strReturn; 
    }

    private function convert_encoding() {   
        $blnIconvPassed = false;
        if (function_exists('iconv') AND
        $strEncodedData = iconv($this->strDefaultCharset, $this->strTargetCharset . '//TRANSLIT', $this->strSourceText)) {
            $blnIconvPassed = true;
            $this->strSourceText = $strEncodedData;
        }
        else if (!$blnIconvPassed AND function_exists('mb_convert_encoding') AND $strEncodedData = @mb_convert_encoding($this->strSourceText, $this->strTargetCharset, $this->strDefaultCharset)) {
            $blnIconvPassed = true;
            $this->strSourceText = $strEncodedData;
        } 
        return $blnIconvPassed; 
    }
}
?>