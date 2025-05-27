import { Button, Nav, NavItem } from "reactstrap";
import { Link, useLocation } from "react-router-dom";
import { useState, useEffect } from "react";

import user1 from "../assets/images/users/user4.jpg";
import probg from "../assets/images/bg/download.jpg";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faUser } from '@fortawesome/free-solid-svg-icons';
import { faPrescriptionBottle, faUserMd, faUserInjured, faPills } from "@fortawesome/free-solid-svg-icons";
import axios  from "axios";
import { faClipboardList } from '@fortawesome/free-solid-svg-icons';
// Define the navigation items
const navigation = [
  {
    title: "Comptes",
    href: "comptes",
    icon: <FontAwesomeIcon icon={faUser} />,
    rolesAllowed: ["Admin"] // Visible only to admin
  },
  {
    title: "Medicaments",
    href: "Medicaments",
    icon: <FontAwesomeIcon icon={faPills} />,
    rolesAllowed: ["Admin", "Pharmacien"] // Visible to admin and pharmacien
  },
  {
    title: "Medecins",
    href: "badges",
    icon: <FontAwesomeIcon icon={faUserMd} />,
    rolesAllowed: ["Admin","Pharmacien"] // Visible to admin and medecin
  },
  {
    title: "Patients",
    href: "buttons",
    icon: <FontAwesomeIcon icon={faUserInjured} />,
    rolesAllowed: ["Admin", "Medecin", "Pharmacien"] // Visible to admin, medecin, and pharmacien
  },
  {
    title: "Pharmaciens",
    href: "cards",
    icon: <FontAwesomeIcon icon={faPrescriptionBottle} />,
    rolesAllowed: ["Admin"] // Visible to admin and pharmacien
  },
  {
    title: "Demandes",
    href: "demande",
    icon: <FontAwesomeIcon icon={faClipboardList} />,
    rolesAllowed: ["Admin", "Medecin", "Pharmacien", "Patient"] // Visible to admin and pharmacien
  }
];


 

const Sidebar = () => {
  // const [currentUser, setCurrentUser] = useState({"id":0,"name":"naziha","email":"turkinaziha2001@gmail.com","email_verified_at":null,"role":"admin","avatar":"http://res.cloudinary.com/esps/image/upload/v1733300323/images/yoaxjx773vx6p9vei0of.jpg","isActive":1,"created_at":"2024-12-04T08:18:50.000000Z","updated_at":"2024-12-04T08:18:50.000000Z"});
  const [currentUser, setCurrentUser] = useState({
    id:" ",
    userName: " ",
  email: " ",
  password: "Abcde123456!",
  role: " ",
  avatar: " ",
  isActive: true,
  clienturi: "string"
  }); // Pour stocker le médicament à modifier

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

        console.log("Fetched user data:", currentUser);
      } catch (error) {
        console.error("Error fetching user data:", error);
      }
    } else {
      console.log("No user data found in localStorage.");
    }
  };


  const location = useLocation();
  // Assuming 'user' is the object you want to store
// const user = { name: 'John Doe', email: 'john@example.com' };

// Correctly stringify the object before storing it
// localStorage.setItem("user", JSON.stringify(user));

useEffect(() => {

  fetchUserData()
//  setCurrentUser(userData)


// const storedUserData = localStorage.getItem("user");
// if (storedUserData) {
//   const user = JSON.parse(storedUserData);
//   console.log("User data in localStorage:", user);
//   setCurrentUser(user);
// }

}, []);

  
  const showMobilemenu = () => {
    document.getElementById("sidebarArea").classList.toggle("showSidebar");
  };
  // let location = useLocation();
  const filteredNavigation = navigation.filter(item =>
    item.rolesAllowed.includes(currentUser?.role)
  );
  return (
    <div>
      <div className="d-flex align-items-center"></div>
      <div
          className="profilebg"
          style={{
            backgroundImage: "url('https://res.cloudinary.com/dyzh0aong/image/upload/v1733319780/462579411_521130227592431_921157361335074824_n_vqloih.png')",
            backgroundRepeat: "no-repeat",
            backgroundSize: "cover", // Ensures the image covers the entire div
            backgroundPosition: "center", // Centers the image
          }}
        >
                <div className="p-3 d-flex">
          <img src={currentUser.avatar} alt="user" width="50" className="rounded-circle" />
          <Button
            color="white"
            className="ms-auto text-white d-lg-none"
            onClick={() => showMobilemenu()}
          >
            <i className="bi bi-x"></i>
          </Button>
        </div>
        <div className="bg-dark text-white p-2 opacity-75">{currentUser.userName}</div>
      </div>
      <div className="p-3 mt-2">
        <Nav vertical className="sidebarNav">
          {filteredNavigation.map((navi, index) => (
            <NavItem key={index} className="sidenav-bg">
              <Link
                to={navi.href}
                className={
                  location.pathname === navi.href
                    ? "active nav-link py-3"
                    : "nav-link text-secondary py-3"
                }
              >
                {navi.icon} 
                {/* <i className={navi.icon}></i> */}
                <span className="ms-3 d-inline-block">{navi.title}</span>
              </Link>
            </NavItem>
          ))}
          {/* <Button
            color="danger"
            tag="a"
            target="_blank"
            className="mt-3"
            href="https://wrappixel.com/templates/materialpro-react-admin/?ref=33"
          >
            Upgrade To Pro
          </Button> */}
        </Nav>
      </div>
    </div>
  );
};

export default Sidebar;
