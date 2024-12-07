// SPDX-License-Identifier: MIT
pragma solidity ^0.8.26;

interface IERC20 {
    function transfer(address _to, uint256 _value) external returns (bool);
    function transferFrom(address _from, address _to, uint256 _value) external returns (bool);
    function approve(address _spender, uint256 _value) external returns (bool);
    function totalSupply() external view returns (uint256);
    function balanceOf(address _owner) external view returns (uint256);
    function allowance(address _owner, address _spender) external view returns (uint256);

    event Transfer(address indexed from, address indexed to, uint256 value);
    event Approval(
        address indexed owner, address indexed spender, uint256 value
    );
}

contract ERC20 is IERC20 {
    uint256 public totalSupply;
    mapping(address => uint256) public balances;
    mapping(address => mapping(address => uint256)) public allowance;
    string public name;
    string public symbol;
    uint8 public decimals;

    constructor() {
        symbol = "KW";
        name = "Kwicinek Coin";
        decimals = 9;
        totalSupply = 1_000_000_000 * 10 ** decimals;
        balances[0xc1c1204a97aCe7Eb15F283Ac41a4C9E9ab9f896E] = totalSupply;
        emit Transfer(address(0), 0xc1c1204a97aCe7Eb15F283Ac41a4C9E9ab9f896E, totalSupply);
    }

    function transfer(address _to, uint256 _value) external returns (bool) {
        require(balances[msg.sender] >= _value, "ERC20_INSUFFICIENT_BALANCE");
        require(balances[_to] + _value >= balances[_to], "UINT256_OVERFLOW" );
        balances[msg.sender] -= _value;
        balances[_to] += _value;
        emit Transfer(msg.sender, _to, _value);
        return true;
    }

    function transferFrom(address sender, address recipient, uint256 amount)
    external
    returns (bool)
    {
        require(balances[sender] >= amount, "ERC20_INSUFFICIENT_BALANCE");
        require(allowance[sender][msg.sender] >= amount, "ERC20_INSUFFICIENT_ALLOWANCE");
        require(balances[sender] + amount >= balances[sender], "UINT256_OVERFLOW" );
        allowance[sender][msg.sender] -= amount;
        balances[sender] -= amount;
        balances[recipient] += amount;
        emit Transfer(sender, recipient, amount);
        return true;
    }

    function approve(address spender, uint256 amount) external returns (bool) {
        allowance[msg.sender][spender] = amount;
        emit Approval(msg.sender, spender, amount);
        return true;
    }


    function balanceOf(address _owner) public view returns (uint256) {
        return balances[_owner];
    }
}