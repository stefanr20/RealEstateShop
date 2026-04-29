import React, { useEffect, useState } from 'react';
import { getProperties } from '../services/api';
import './Properties.css';

function Properties() {
  const [properties, setProperties] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    getProperties()
      .then(response => {
        setProperties(response.data);
        setLoading(false);
      })
      .catch(error => {
        console.error('Error fetching properties:', error);
        setLoading(false);
      });
  }, []);

  if (loading) return <div className="loading">Loading properties...</div>;

  return (
    <div className="properties-page">
      <div className="page-header">
        <p className="page-subtitle">EXCLUSIVE LISTINGS</p>
        <h1 className="page-title">Our Properties</h1>
      </div>

      {properties.length === 0 ? (
        <p className="no-data">No properties available at the moment.</p>
      ) : (
        <div className="properties-grid">
          {properties.map(property => (
            <div className="property-card" key={property.id}>
              <div className="property-image">
                <div className="image-placeholder">🏛</div>
              </div>
              <div className="property-info">
                <h3 className="property-title">{property.title}</h3>
                <p className="property-description">{property.description}</p>
                <div className="property-details">
                  <span className="property-price">{property.price}</span>
                  <span className="property-location">
                    {property.city}, {property.country}
                  </span>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

export default Properties;