pragma solidity ^0.4.20;

contract Owned {
    address owner;
    event OwnerChanged(address from, address to);
    
    function Owned() public {
        owner = msg.sender;
    }
    
    function changeOwner(address _newOwner) onlyowner public {
        address oldOwner = owner;
        owner = _newOwner;
        emit OwnerChanged(oldOwner, owner);
    }
    
    modifier onlyowner {
        if(owner == msg.sender) 
            _;
    }
}

contract Sware is Owned {
    uint public fee = 0;
    event Wrote(string _content);
    
    function changeFee(uint _fee) onlyowner public {
        fee = _fee;
    }
    
    function write(string _content) payable public{
        require(msg.value >= fee);
        emit Wrote(_content);
    }
    
    function withdraw() onlyowner public {
        msg.sender.transfer(this.balance);
    }
}
