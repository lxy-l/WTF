kubeadm reset
rm -fr /data/etcd/*
rm -fr /etc/kubernetes/*
rm -fr ~/.kube/

ifconfig cni0 down
ip link delete cni0
ifconfig flannel.1 down
ip link delete flannel.1
rm -rf /var/lib/cni/
rm -f /etc/cni/net.d/*
