import axios from 'axios';
import React, { useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';

const FormComponent = ({ data, buttonText, sendUrl }) => {
    const location = useLocation();
    const { state } = location;
    const navigate = useNavigate();

    const [formData, setFormData] = useState({});

    console.log("formData", formData, state);

    const handleChange = ({ currentTarget: input }) => {
        setFormData({ ...formData, [input.name]: input.value });
    }

    const config = {
        headers: { Authorization: `Bearer ${localStorage.getItem("token")}` }
    };

    const handleSubmit = () => {
        if (state && state.data && state.data.id) {
            // Handle PUT request
            axios.put(`${sendUrl}/${state.data.id}`, formData, config)
                .then(response => {
                    navigate("/salerecords");
                    console.log(response.data);
                })
                .catch(err => {
                    console.log(err);
                });
        } else {
            // Handle POST request
            axios.post(sendUrl, formData, config)
                .then(response => {
                    if (buttonText === "Login") {
                        localStorage.setItem("token", response.data.data.token);
                    }
                    navigate("/salerecords");
                    console.log(response.data);
                })
                .catch(err => {
                    console.log(err);
                });
        }
    }

    useEffect(() => {
        if (state && state.data) {
            setFormData(state.data);
        }
    }, [state]);

    return (
        <div className='border'>
            <div className='flex flex-col p-8 gap-4'>
                {data.map((dat) => (
                    <div key={dat.name}>
                        <label className='font-bold'>{dat.name}</label>
                        <input 
                            className='px-4 py-2 border rounded-md'
                            name={dat.name}
                            type={dat.type || "text"} // Use "text" as the default type
                            placeholder={dat.name}
                            value={formData[dat.name] || ''}
                            onChange={handleChange}
                        />
                    </div>
                ))}
                <button
                    className='bg-blue-500 py-2 rounded-xl text-white font-bold mt-2'
                    onClick={handleSubmit}
                >
                    {buttonText}
                </button>
            </div>
        </div>
    );
}

export default FormComponent;
