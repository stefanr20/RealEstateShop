import React, { useEffect, useState } from 'react';
import { getCustomers } from '../services/api';
import './Customers.css';

function Customers() {
  const [customers, setCustomers] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    getCustomers()
      .then(response => {
        setCustomers(response.data);
        setLoading(false);
      })
      .catch(error => {
        console.error('Error fetching customers:', error);
        setLoading(false);
      });
  }, []);

  if (loading) return <div className="loading">Loading customers...</div>;

  return (
    <div className="customers-page">
      <div className="page-header">
        <p className="page-subtitle">OUR CLIENTS</p>
        <h1 className="page-title">Customers</h1>
      </div>

      {customers.length === 0 ? (
        <p className="no-data">No customers available at the moment.</p>
      ) : (
        <div className="customers-grid">
          {customers.map(customer => (
            <div className="customer-card" key={customer.id}>
              <div className="customer-avatar">
                {customer.firstName?.charAt(0)}{customer.lastName?.charAt(0)}
              </div>
              <div className="customer-info">
                <h3 className="customer-name">
                  {customer.firstName} {customer.lastName}
                </h3>
                <p className="customer-email">{customer.email}</p>
                <p className="customer-phone">{customer.phone}</p>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

export default Customers;