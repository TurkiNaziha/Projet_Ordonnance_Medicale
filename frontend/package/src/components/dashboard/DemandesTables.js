import React, { useState, useEffect } from 'react';
import {
  Card,
  CardBody,
  Table,
  Modal,
  ModalHeader,
  ModalBody,
  ModalFooter,
  Button,
  Form,
  FormGroup,
  Label,
  Input,
} from 'reactstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrashAlt, faEdit, faSyncAlt } from '@fortawesome/free-solid-svg-icons';
import axios from 'axios';
import Swal from 'sweetalert2';
const DemandeTable = ({ demandes, demande , setDemandes, setDemande}) => {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedDemande, setSelectedDemande] = useState({});
  const [medicaments, setMedicaments] = useState([]);
  
  const [storedMedicaments, setStoredMedicaments] = useState([]);
  const [storedMontant, setStoredMontant] = useState(0);
  const [storedusername, setStoredUsername] = useState(0);
    
    const [selectedMedicaments, setSelectedMedicaments] = useState([]);
    const [quantities, setQuantities] = useState({});
    const [totalAmount, setTotalAmount] = useState(0);
   const [currentUser, setCurrentUser] = useState({
      id:" ",
      userName: " ",
    email: " ",
    password: "Abcde123456!",
    role: " ",
    avatar: " ",
    isActive: true,
    clienturi: "string"
    });


    const handleCheckboxChange = (e, medicamentId) => {
      const { checked } = e.target;
      if (checked) {
        setSelectedMedicaments((prev) => [...prev, medicamentId]);
      } else {
        setSelectedMedicaments((prev) =>
          prev.filter((id) => id !== medicamentId)
        );
      }
    };
  
    const handleQuantityChange = (e, medicamentId) => {
      const quantity = Math.max(1, parseInt(e.target.value, 10) || 1); // Ensure minimum of 1
      setQuantities((prev) => ({ ...prev, [medicamentId]: quantity }));
    };
  
    const calculateTotal = () => {
      const total = selectedMedicaments.reduce((sum, medicamentId) => {
        const medicament = medicaments.find((med) => med.id === medicamentId);
        const quantity = quantities[medicamentId] || 1;
        return sum + medicament.prix * quantity;
      }, 0);
      setTotalAmount(total);
    };

//   const [demande, setDemande] = useState({
//     idPatient: '',
//     date: '',
//     medicament: '',
//     quantite: 0,
//     montant: 0,
//     state: 'NotApproved',
//   });

  const fetchUserData = async () => {
    const userData = localStorage.getItem("user");
    console.log("User data in localStorage:", userData);
    
    if (userData) {
      // Remove quotes around the string if present
      const parsedUserData = userData.replace(/"/g, ''); // This removes the double quotes
      
      try {
        // Pass username as part of the URL
        const res = await axios.get(`http://localhost:5156/api/Account/GetByUserName/${parsedUserData}`);
        // setCurrentUser(res.data);


        const roles = res.data.roles;

        // Access the first role
        const firstRole = roles[0];
            
           
        setCurrentUser({
          // Spread the previous state
          id: res.data.id,
          role: firstRole,
          userName: res.data.userName,
          email: res.data.email,
          avatar: res.data.avatar,
        });
          if(firstRole=="Patient")
          {
            // setDemande({...Demande,patientName:currentUser.userName})
          }
          setStoredUsername(res.data.userName)
        console.log("Fetched user data:", currentUser);
        fetchDemandes(res.data.userName, firstRole)
      } catch (error) {
        console.error("Error fetching user data:", error);
      }
    } else {
      console.log("No user data found in localStorage.");
    }
  };


  const fetchDemandes = async (usename, role) => {
    console.log("use",usename);
      try {
        if(role=="Medecin")
        {
          const res = await axios.get(`http://localhost:5156/api/Demand/by-medecin/${usename}`);
          setDemandes(res.data);
          console.log(res.data)
        }
        else if(role=="Patient") {
          const res = await axios.get(`http://localhost:5156/api/Demand/by-patient/${usename}`);
          setDemandes(res.data);
          console.log(res.data)
        }
        else{
          const res = await axios.get('http://localhost:5156/api/Demand');
          setDemandes(res.data);
          console.log(res.data)
        }
       
      } catch (error) {
        console.error('Error fetching demandes:', error);
      }
    };

  useEffect(() => {
    // Fetch demandes
    

    // Fetch medicaments
    const fetchMedicaments = async () => {
      try {
        const res = await axios.get('http://localhost:5156/api/Medicament');
        setMedicaments(res.data);
        console.log(res.data)
      } catch (error) {
        console.error('Error fetching medicaments:', error);
      }
    };
    fetchUserData();
    // fetchDemandes();
    fetchMedicaments();
  }, [setDemandes]);

  const handleSave = async () => {
    try {
      if (demande.id) {
        // Update existing demande
        await axios.put(`http://localhost:8000/api/demandes/${demande.id}`, demande);
      } else {
        // Create new demande
        await axios.post('http://localhost:8000/api/demandes', demande);
      }
      setIsModalOpen(false);
      fetchDemandes();
    } catch (error) {
      console.error('Error saving demande:', error);
    }
  };

  const handleDelete = async (id) => {
    if (window.confirm('Êtes-vous sûr de vouloir supprimer cette demande ?')) {
      try {
        await axios.delete(`http://localhost:5156/api/Demand/${id}`);
        fetchDemandes();
      } catch (error) {
        console.error('Error deleting demande:', error);
      }
    }
  };

  const handleEdit = (d) => {
    setDemande(d);
    console.log("d",d)
    setIsModalOpen(true);
  };

  const calculateMontant = () => {
    if (demande.medicament && demande.quantite) {
      const selectedMedicament = medicaments.find(
        (med) => med.id === parseInt(demande.medicament)
      );
      return selectedMedicament ? selectedMedicament.prix * demande.quantite : 0;
    }
    return 0;
  };

  const updateState = async (d, currentState) => {
    // Determine the next state
    // const nextState = currentState === 0 
    //   ? 1 ;// Move from Not Approved to Approved
      // : currentState === 1
      // ? 2 // Move from Approved to Done
      // : 0; // Reset to Not Approved
      const nextState =1;
    try {
      // Call API to update state
      await axios.patch(`http://localhost:5156/api/Demand/ChangeState/${d.id}?state=${nextState}`);
  
      // Refresh the list of demandes after updating the state
      const res = await axios.get(`http://localhost:5156/api/Demand/by-medecin/${currentUser.userName}`);
      setDemandes(res.data)
    } catch (error) {
      console.error('Error updating state:', error);
    }
  };
  

  
  const setMedicamentAndState = async () => {
    // Calculate the total amount
    const total = selectedMedicaments.reduce((sum, medicamentId) => {
      const medicament = medicaments.find((med) => med.id === medicamentId);
      const quantity = quantities[medicamentId] || 1;
      return sum + medicament.prix * quantity;
    }, 0);
  
    // Update states with selected medicaments and total montant
    setStoredMedicaments(
      selectedMedicaments.map((id) => ({
        id,
        quantity: quantities[id] || 1,
      }))
    );
    setStoredMontant(total);
  
    console.log("mt", total);
    console.log("idD", demande.id);

  
    try {
      // Send the PATCH request to update the montant
      const resp = await axios.patch(`http://localhost:5156/api/Demand/ChangeMontant/${demande.id}?montant=${total}`);
      console.log("Montant updated successfully", resp.data);
      if(demande.etat==1)
      {
        const state=2;
        const resp2 = await axios.patch(`http://localhost:5156/api/Demand/ChangeState/${demande.id}?state=${state}`);
      }
      else{
           Swal.fire({
                        icon: "error",
                        title: "Oops...",
                        text: "this demand is not approved by the doctor yet",
                        footer: '<a href="#">Why do I have this issue?</a>'
                    });
      }
     

    } catch (error) {
      console.error("Error updating montant", error);
    }
  
    // Optionally, close the modal
    setIsModalOpen(false);
  
    fetchDemandes(); // Refresh the list of demandes
  };

  
  const handleClose=()=>
  {
    setStoredMedicaments([])
        setStoredMontant(0)
        setIsModalOpen(false)
  }

  return (
    <div>
      <Card>
        <CardBody>
          <Table responsive>
            <thead>
              <tr>
                <th>Nom Patient</th>
                <th>Date</th>
                <th>Medecin Name</th> 
                {/* {(currentUser.role === 'Admin' || currentUser.role === 'Pharmacien') && ( */}
  <th>Médicaments</th>
 {/* )}  */}

                 
                <th>Montant</th>
                <th>État</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {demandes.map((d, index) => (
                <tr key={index}>
                  <td>{d.patientName}</td>
                  <td>{d.date}</td>
                  <td>{d.medecinName}</td>


                  <td>
      {/* Button to Open Modal */}
      {/* <button onClick={() => setIsModalOpen(true)}>Select Medicaments</button> */}
      {(currentUser.role === 'Admin' || currentUser.role === 'Pharmacien') && (
          <td>
            <Button color="primary" onClick={() => handleEdit(d)}>
              Select Medicaments
            </Button>
          </td>
        )}

      {/* Modal */}
      {/* {isModalOpen && (
        <div className="modal">
          <div className="modal-content">
            <h2>Select Medicaments</h2>
            <div style={{ maxHeight: "300px", overflowY: "auto" }}>
              {medicaments.map((med) => (
                <div
                  key={med.id}
                  style={{
                    display: "flex",
                    alignItems: "center",
                    marginBottom: "10px",
                  }}
                >
                         <input
                    type="checkbox"
                    id={`medicament-${med.id}`}
                    checked={selectedMedicaments.includes(med.id)}
                    onChange={(e) => handleCheckboxChange(e, med.id)}
                    style={{ marginRight: "10px" }}
                  />
                  <label
                    htmlFor={`medicament-${med.id}`}
                    style={{ marginRight: "10px" }}
                  >
                    {med.nom} - {med.prix}€
                  </label>
                  {selectedMedicaments.includes(med.id) && (
                    <input
                      type="number"
                      min="1"
                      value={quantities[med.id] || 1}
                      onChange={(e) => handleQuantityChange(e, med.id)}
                      style={{ width: "60px", marginLeft: "10px" }}
                    />
                  )}
                </div>
              ))}
            </div>

            {/* Footer with Calculate Button */}
            {/* <div className="modal-footer">
              <button onClick={calculateTotal}>Calculate</button>
              <span style={{ marginLeft: "10px" }}>Total: {totalAmount}€</span>
              <button
                style={{ marginLeft: "20px" }}
                onClick={() => setIsModalOpen(false)}
              >
                Close
              </button>
            </div>
          </div>
        </div>
      )}  */}
    </td>








                  <td>{d.montant} €</td>
                  {/* <td>{d.quantite}</td> */}
                 
                  <td>
                  <Button
  size="sm"
  color={d.etat === 0 ? 'danger' : d.etat === 1 ? 'warning' : d.etat === 2 ? 'info' : 'yellow'}
  disabled={currentUser.role !== 'Medecin' } // Disable button if the user is not a "Medecin"
  onClick={() => updateState(d, d.state)}
>
  {d.state} <FontAwesomeIcon icon={faSyncAlt} />
</Button>

                  </td>
                  <td>
                    <Button
                      color="warning"
                      size="sm"
                      className="me-2"
                      onClick={() => handleEdit(d)}
                    >
                      <FontAwesomeIcon icon={faEdit} />
                    </Button>
                    <Button
                      color="danger"
                      size="sm"
                      onClick={() => handleDelete(d.id)}
                    >
                      <FontAwesomeIcon icon={faTrashAlt} />
                    </Button>
                  </td>
                </tr>
              ))} 
            </tbody>
          </Table>
        </CardBody>
      </Card>

      {/* Modal */}
      <Modal isOpen={isModalOpen} toggle={() => setIsModalOpen(false)}>
        <ModalHeader toggle={() => setIsModalOpen(false)}>
          {/* {d.id ? "Modifier une demande" : "Ajouter une demande"} */}
        </ModalHeader>
        <ModalBody>
          <Form>
            <FormGroup>
              <Label for="medicaments">Medicaments</Label>
              <div style={{ maxHeight: "300px", overflowY: "auto" }}>
                {medicaments.map((med) => (
                  <div
                    key={med.id}
                    style={{
                      display: "flex",
                      alignItems: "center",
                      marginBottom: "10px",
                    }}
                  >
                    <Input
                      type="checkbox"
                      id={`medicament-${med.id}`}
                      checked={selectedMedicaments.includes(med.id)}
                      onChange={(e) => handleCheckboxChange(e, med.id)}
                      style={{ marginRight: "10px" }}
                    />
                    <Label
                      htmlFor={`medicament-${med.id}`}
                      style={{ marginRight: "10px" }}
                    >
                      {med.nom} - {med.prix}€
                    </Label>
                    {selectedMedicaments.includes(med.id) && (
                      <Input
                        type="number"
                        min="1"
                        value={quantities[med.id] || 1}
                        onChange={(e) => handleQuantityChange(e, med.id)}
                        style={{ width: "60px", marginLeft: "10px" }}
                      />
                    )}
                  </div>
                ))}
              </div>
            </FormGroup>
          </Form>
        </ModalBody>
        <ModalFooter>
          <Button color="primary" onClick={calculateTotal}>
            Calculate
          </Button>
          <span style={{ marginLeft: "10px", fontWeight: "bold" }}>
            Total: {totalAmount}€
          </span>
          <Button
            color="secondary"
            onClick={handleClose}
            style={{ marginLeft: "20px" }}
          >
            Close
          </Button>
          <Button color="primary" onClick={setMedicamentAndState}>
            Enregistrer
          </Button>
        </ModalFooter>
      </Modal>
    </div>
  );
};

export default DemandeTable;