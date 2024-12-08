import React, { useState } from 'react';
import LiveComponent from './components/LiveComponent';
import HomeComponent from './components/HomeComponent';
import DetailsComponent from './components/DetailsComponent';
import WalletsComponent from './components/WalletsComponent';

function App() {
    const [activeTab, setActiveTab] = useState('Home');

    return (
        <div>
            {/* Menu */}
            <div style={{ display: 'flex', borderBottom: '1px solid #ccc' }}>
                <button onClick={() => setActiveTab('Home')} style={tabStyle(activeTab === 'Home')}>
                    Home
                </button>
                <button onClick={() => setActiveTab('Live')} style={tabStyle(activeTab === 'Live')}>
                    Live
                </button>
                <button onClick={() => setActiveTab('Details')} style={tabStyle(activeTab === 'Details')}>
                    Details
                </button>
                <button onClick={() => setActiveTab('Wallets')} style={tabStyle(activeTab === 'Wallets')}>
                    Wallets
                </button>
            </div>

            {/* Content */}
            {activeTab === 'Home' && (
                <div>
                    <HomeComponent />
                </div>
            )}

            {activeTab === 'Live' && (
                <div>
                    <LiveComponent />
                </div>
            )}

            {activeTab === 'Details' && (
                <div>
                    <DetailsComponent />
                </div>
            )}

            {activeTab === 'Wallets' && (
                <div>
                    <WalletsComponent />
                </div>
            )}
        </div>
    );
}

const tabStyle = (isActive) => ({
    padding: '10px 20px',
    border: 'none',
    borderBottom: isActive ? '2px solid blue' : '2px solid transparent',
    background: 'none',
    cursor: 'pointer',
    outline: 'none',
    fontWeight: isActive ? 'bold' : 'normal',
});

export default App;
