package com.cirrious.exampleApp;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;

public class ExampleAppActivity extends Activity {
	private Button goButton;
    /** Called when the activity is first created. */
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.main);
        this.goButton = (Button)this.findViewById(R.id.button1);
        
        this.goButton.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
              startActivity(new Intent(ExampleAppActivity.this, ChildActivity.class));
            }
          });
    }
}