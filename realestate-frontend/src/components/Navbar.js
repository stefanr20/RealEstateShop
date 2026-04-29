import React from 'react';
import { Link } from 'react-router-dom';
import './Navbar.css';

function Navbar() {
  return (
    <nav className="navbar">
      <div className="navbar-brand">
        <Link to="/">Bepd.RealEstate</Link>
      </div>
      <div className="navbar-links">
        <Link to="/">Home</Link>
        <Link to="/properties">Properties</Link>
        <Link to="/customers">Customers</Link>
        <Link to="/properties" className="navbar-contact">Contact</Link>
      </div>
    </nav>
  );
}

export default Navbar;