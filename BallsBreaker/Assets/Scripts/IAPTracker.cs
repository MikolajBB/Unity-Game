using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using System;
using TMPro;

public class IAPTracker : MonoBehaviour, IStoreListener
{

    public static IAPTracker Instance;
    public GameObject textMesh;
    private TextMeshProUGUI GemText;

    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    ////buy multiple times
    //public static string kProductIDConsumable = "consumable";

    ////buy only once
    //public static string kProductIDNonConsumable = "nonconsumable";

    public static string PRODUCT_GEM_30 = "gem30";
    public static string PRODUCT_GEM_100 = "gem100";
    public static string PRODUCT_GEM_10 = "gem10";

    //// Google Play Store-specific product identifier subscription product.
    //private static string kProductNameGooglePlaySubscription = "com........";


    private void Awake()
    {
        Instance = this;
        if (!PlayerPrefs.HasKey("Gem"))
        {
            PlayerPrefs.SetInt("Gem", 100);
        }
    }

    public void Add10Gem()
    {
        BuyProductID(PRODUCT_GEM_10);
    }

    public void Add30Gem()
    {
        BuyProductID(PRODUCT_GEM_30);
    }

    public void Add100Gem()
    {
        BuyProductID(PRODUCT_GEM_100);
    }

    void Start()
    {
        GemText = textMesh.GetComponent<TextMeshProUGUI>();
        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(PRODUCT_GEM_30, ProductType.Consumable);
        builder.AddProduct(PRODUCT_GEM_10, ProductType.Consumable);
        builder.AddProduct(PRODUCT_GEM_100, ProductType.Consumable);

        // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
        // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {  
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
            // retrying initiailization.
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }


    void Update()
    {
        var gemValue = PlayerPrefs.GetInt("Gem");

        GemText.SetText(gemValue.ToString());
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_GEM_30, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("30 GEM: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            var gemValue = PlayerPrefs.GetInt("Gem");
            gemValue += 30;
            PlayerPrefs.SetInt("Gem", gemValue);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_GEM_10, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("10 gem: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            var gemValue = PlayerPrefs.GetInt("Gem");
            gemValue += 10;
            PlayerPrefs.SetInt("Gem", gemValue);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_GEM_100, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("100 gem: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            var gemValue = PlayerPrefs.GetInt("Gem");
            gemValue += 100;
            PlayerPrefs.SetInt("Gem", gemValue);
        }
        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        // Return a flag indicating whether this product has completely been received, or if the application needs 
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
        // saving purchased products to the cloud, and when that save is delayed. 
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("Initialized IN-APP purchasing system: PASS");

        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }
}
