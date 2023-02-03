export http_proxy=http://172.28.192.1:1080;
export https_proxy=http://172.28.192.1:1080;
export no_proxy=127.0.0.1,localhost,172.17.191.4,10.96.0.0/12,10.244.0.0/16
env |grep -i proxy;
