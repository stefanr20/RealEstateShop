import React from 'react';
import { Link } from 'react-router-dom';
import './Home.css';


function Home() {
  return (
    <div className="home">
      <div className="hero">
        <div className="hero-left">
          <p className="hero-tag">Live Exceptionally</p>
          <h1 className="hero-title">
            Luxury is<br />more than<br />a home
          </h1>
          <p className="hero-description">
            Discover exclusive properties where design,
            location, and lifestyle meet.
          </p>
          <Link to="/properties" className="hero-explore">
            Explore Listings →
          </Link>
        </div>
        <div className="hero-right">
  <img src={require('../house.webp')} alt="Luxury Villa" className="hero-image" />
  <div className="property-preview">
    <div className="preview-image">🏡</div>
    <div>
      <p className="preview-name">Sarai Villas</p>
      <p className="preview-details">$3,000,000 · Dubai</p>
    </div>
  </div>
</div>
      </div>
      <div className="hero-section2">
        <h2 className="section2-title">
          From <span>coastal villas</span> to city penthouses
        </h2>
      </div>
    </div>
  );
}

export default Home;