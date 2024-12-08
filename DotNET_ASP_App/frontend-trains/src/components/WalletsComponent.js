import React, { useEffect, useState } from 'react';

const WalletsComponent = () => {
    const [wallets, setWallets] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchWallets = async () => {
            try {
                const backendHost = `${window.location.protocol}//${window.location.hostname}:8080`;
                const response = await fetch(`${backendHost}/api/wallets`);
                if (!response.ok) {
                    throw new Error('Failed to fetch wallets');
                }
                const data = await response.json();
                setWallets(data.balances);
            } catch (err) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };

        fetchWallets();
    }, []);

    if (loading) {
        return <div>Loading wallets...</div>;
    }

    if (error) {
        return <div>Error: {error}</div>;
    }

    return (
        <div>
            <h2>Wallet Balances</h2>
            <table style={{ width: '100%', borderCollapse: 'collapse', border: '1px solid #ccc' }}>
                <thead>
                    <tr>
                        <th style={cellStyle}>Wallet ID</th>
                        <th style={cellStyle}>Balance</th>
                    </tr>
                </thead>
                <tbody>
                    {wallets.map((wallet) => (
                        <tr key={wallet.id}>
                            <td style={cellStyle}>{wallet.id}</td>
                            <td style={cellStyle}>{wallet.balance.toFixed(10)}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

const cellStyle = {
    border: '1px solid #ccc',
    padding: '10px',
    textAlign: 'center',
};

export default WalletsComponent;
